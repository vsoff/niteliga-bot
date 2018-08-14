using Newtonsoft.Json;
using NiteLigaLibrary;
using NiteLigaLibrary.Classes;
using NiteLigaLibrary.Database;
using NiteLigaLibrary.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebServer.Classes
{
    public static class ServerGamesManager
    {
        private static object _locker = new object();
        private const int MAXGAMES = 3;
        private static Dictionary<long, GameManager> Games = new Dictionary<long, GameManager>();

        public static void Initialize()
        {
            lock (_locker)
            {
                List<GameArchive> notEndedGames;
                using (var db = new NiteLigaContext())
                {
                    // Получаем список незавершённых игр
                    notEndedGames = db.GameArchives.Where(x => x.EndDate == null).ToList();

                    // Восстанавливаем прогресс всех незавершённых игр, и запускаем их
                    foreach (var g in notEndedGames)
                    {
                        GameManager gm = new GameManager(g.GameProject.GetConfig(), g.GameProject.GetSetting());
                        if (!Games.ContainsKey(g.Id))
                        {
                            Games[g.GameProjectId] = gm;
                            Games[g.GameProjectId].Restore(JsonConvert.DeserializeObject<GameBackupModel>(g.Log, new GameBackupConverter()));
                            GameWorkOn(g.GameProjectId, g.Id);
                        }
                    }
                }
            }
        }

        public static void StartGame(long gameProjectId, bool isTestRun)
        {
            lock (_locker)
            {
                if (Games.Count > MAXGAMES)
                    throw new Exception($"Максимальное количество одновременно проводимых игр ({MAXGAMES}) превышено");

                // First check
                if (Games.ContainsKey(gameProjectId))
                    throw new Exception("Нельзя начать игру, так как она уже была начата");

                GameArchive gameArchive;
                GameProject gameProject;
                using (var db = new NiteLigaContext())
                {
                    gameProject = db.GameProjects.Single(x => x.Id == gameProjectId);

                    // Проверяем, были ли проведены НЕ ТЕСТОВЫЕ игры по этой конфигурации
                    if (gameProject.GameArchives.Where(x => x.IsTestRun == false).Count() != 0)
                        throw new Exception("Эта игра уже проводилась");

                    // Проверяем конфигурацию на наличие ошибок
                    List<string> verifyErrors;
                    if (!gameProject.VerifyData(out verifyErrors))
                        throw new Exception($"Игра неправильно сконфигурирована ({verifyErrors.Aggregate((a, b) => a + ',' + b)})");

                    // Добавляем запись в архив
                    gameArchive = db.GameArchives.Add(new GameArchive
                    {
                        StartDate = DateTime.Now,
                        IsTestRun = isTestRun,
                        Log = "{}",
                        GameProjectId = gameProject.Id
                    });
                    db.SaveChanges();
                }

                GameManager gm = new GameManager(gameProject.GetConfig(), gameProject.GetSetting());

                // Second check
                if (Games.ContainsKey(gameProjectId)) return;

                Games[gameProjectId] = gm;

                // Запускаем игру
                Games[gameProjectId].Start();
                GameWorkOn(gameProjectId, gameArchive.Id);
            }
        }

        public static void StopGame(long gameProjectId)
        {
            if (!Games.ContainsKey(gameProjectId))
                throw new Exception("Нет игры с таким id");

            Games[gameProjectId].Stop();
        }

        public static void AbortGame(long gameProjectId)
        {
            if (!Games.ContainsKey(gameProjectId))
                throw new Exception("Нет игры с таким id");

            Games[gameProjectId].Abort();
        }

        private const int SaveDelayMin = 1;
        private const int IterationDelayMs = 1000;

        private static void GameWorkOn(long gameProjectId, long gameArchiveId)
        {
            Task task = Task.Run(() =>
            {
                GameManager gm = Games[gameProjectId];
                DateTime lastSaveTime = DateTime.Now;

                // Основной цикл игры
                while (gm.GameStatus == GameStatusType.InProgress || gm.GameStatus == GameStatusType.Stopped)
                {
                    // Просчитываем один фрейм игры
                    gm.Iterate();

                    // Сохраняем игру, если прошло нужное время
                    if (DateTime.Now > lastSaveTime.AddMinutes(SaveDelayMin))
                    {
                        using (var db = new NiteLigaContext())
                        {
                            GameArchive ga = db.GameArchives.Single(x => x.Id == gameArchiveId);
                            ga.Log = gm.GetLastBackup();
                            db.SaveChanges();
                        }
                        lastSaveTime = DateTime.Now;
                    }

                    // Делаем паузу
                    Thread.Sleep(IterationDelayMs);
                }

                // Сохраняем время завершения игры и самый последний лог
                using (var db = new NiteLigaContext())
                {
                    GameArchive ga = db.GameArchives.Single(x => x.Id == gameArchiveId);
                    ga.Log = gm.GetLastBackup();
                    ga.EndDate = DateTime.Now;
                    db.SaveChanges();
                }

                // Убираем игру из общего списка
                Games.Remove(gameProjectId);
            });
        }
    }
}