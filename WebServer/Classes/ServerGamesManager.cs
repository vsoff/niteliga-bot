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
        private const int MAXGAMES = 3;
        private static Dictionary<long, GameManager> Games = new Dictionary<long, GameManager>();

        public static void Initialize()
        {
            // TODO: Надо реализовать возможность сохранения и загрузки игры

            //List<GameArchive> notEndedGames;
            //using (var db = new NiteLigaContext())
            //{
            //    // Получаем список незавершённых игр
            //    notEndedGames = db.GameArchives.Where(x => x.EndDate == null).ToList();

            //    // Восстанавливаем прогресс всех незавершённых игр, и запускаем их
            //    foreach (var g in notEndedGames)
            //    {
            //        Games[g.Id] = new GameManager(g.GameProject.GetConfig(), g.GameProject.GetSetting());
            //        Games[g.Id].Restore();
            //        GameWorkOnAsync(g.Id);
            //    }
            //}
        }

        public static void StartGame(long id)
        {
            if (Games.Count > MAXGAMES)
                throw new Exception($"Максимальное количество одновременно проводимых игр ({MAXGAMES}) превышено");

            // Если игры нету в списке, то добавляем
            if (!Games.ContainsKey(id))
            {
                bool isFirstLaunch;
                GameProject gameProject;
                using (var db = new NiteLigaContext())
                {
                    gameProject = db.GameProjects.Single(x => x.Id == id);
                    isFirstLaunch = gameProject.GameArchives.Where(x => x.IsTestRun == false).Count() == 0;
                }

                List<string> verifyErrors;
                if (!gameProject.VerifyData(out verifyErrors))
                    throw new Exception($"Игра неправильно сконфигурирована");

                if (!isFirstLaunch)
                    throw new Exception($"Эта игра уже проводилась");

                GameManager gm = new GameManager(gameProject.GetConfig(), gameProject.GetSetting());

                // Second check
                if (Games.ContainsKey(id)) return;

                Games[id] = gm;
            }

            // Запрещаем запускать уже запущенные игры
            if (Games[id].GameStatus != GameStatusType.Created)
                throw new Exception("Нельзя начать игру, так как она уже была начата");

            // Запускаем игру
            Games[id].Start();
            GameWorkOn(id);
        }

        public static void StopGame(long id)
        {
            if (!Games.ContainsKey(id))
                throw new Exception("Нет игры с таким id");

            Games[id].Stop();
        }

        public static void AbortGame(long id)
        {
            if (!Games.ContainsKey(id))
                throw new Exception("Нет игры с таким id");

            Games[id].Abort();
        }

        private static void GameWorkOn(long id)
        {
            Task task = Task.Run(() =>
            {
                GameManager gm = Games[id];

                while (gm.GameStatus == GameStatusType.InProgress || gm.GameStatus == GameStatusType.Stopped)
                {
                    // Просчитываем один фрейм игры
                    gm.Iterate();
                    // Делаем паузу на 1 секунду
                    Thread.Sleep(1000);
                }

                Games.Remove(id);
            });
        }
    }
}