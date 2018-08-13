using Newtonsoft.Json;
using NiteLigaLibrary;
using NiteLigaLibrary.Classes;
using NiteLigaLibrary.Database;
using NiteLigaLibrary.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServer.Classes;
using WebServer.Models;

namespace WebServer.Controllers
{
    //[Authorize(Roles = "organizer")]
    public class GameController : ApiController
    {
        // POST: api/Game
        [HttpPost]
        public object Create()
        {
            try
            {
                GameProject newGame;
                DateTime dt = DateTime.Now;
                using (var db = new NiteLigaContext())
                {
                    newGame = db.GameProjects.Add(new GameProject
                    {
                        CreateDate = dt,
                        UpdateDate = dt,
                        Caption = $"Новая игра ({dt.ToShortDateString()})",
                        Setting = JsonConvert.SerializeObject(new GameSetting
                        {
                            GameClosingDurationMin = 5,
                            GameDurationMin = 20,
                            Hint1DelaySec = 15,
                            Hint2DelaySec = 15,
                            TaskDropDelaySec = 10,
                            SecondsDelayStart = 10,
                            TeamIds = new List<int>()
                        }),
                        Config = JsonConvert.SerializeObject(new GameConfigModel
                        {
                            Address = "Адрес проведения игры",
                            GameDate = DateTime.Now.Date,
                            Description = "Описание игры на " + DateTime.Now.Date,
                            Tasks = new List<GameTask>() {
                                new GameTask {
                                    Lat = 33,
                                    Lon = 44,
                                    Address = "ул. Тест, дом 1",
                                    Task = "Формулировка задания 1",
                                    Hint1 = "Подсказка задания 1",
                                    Hint2 = "Слив адреса задания 1",
                                    Code = "1",
                                },
                                new GameTask {
                                    Lat = 55,
                                    Lon = 66,
                                    Address = "ул. Тест, дом 2",
                                    Task = "Формулировка задания 2",
                                    Hint1 = "Подсказка задания 2",
                                    Hint2 = "Слив адреса задания 2",
                                    Code = "2",
                                }
                            },
                            TaskGrid = new List<List<int>>()
                        }),
                    });
                    db.SaveChanges();
                }
                return new { data = newGame, message = "New Game created successfully" };
            }
            catch
            {
                return new { error = "An error occurred while creating the Game" };
            }
        }

        // GET: api/Game/5
        [HttpGet]
        public object Get(int id)
        {
            try
            {
                GameProject game;
                using (var db = new NiteLigaContext())
                {
                    game = db.GameProjects.Single(x => x.Id == id);
                }
                return new { data = game, message = "Game getted successfully" };
            }
            catch
            {
                return new { error = "An error occurred while getting the Game" };
            }
        }

        // POST: api/Game/5/Update
        [HttpPost]
        public object Update(int id, GameProjectModel model)
        {
            try
            {
                GameProject game;
                using (var db = new NiteLigaContext())
                {
                    game = db.GameProjects.Single(x => x.Id == id);
                    game.UpdateDate = DateTime.Now;
                    game.Caption = model.Caption;
                    game.Setting = model.Setting;
                    game.Config = model.JSON;
                    db.SaveChanges();
                }
                return new { message = "Game updated successfully" };
            }
            catch
            {
                return new { error = "An error occurred while updating the Game" };
            }
        }

        // POST: api/Game/5/Verify
        [HttpPost]
        public object Verify(int id)
        {
            try
            {
                GameProject game;
                using (var db = new NiteLigaContext())
                    game = db.GameProjects.Single(x => x.Id == id);

                List<string> errorList;
                bool isGood = game.VerifyData(out errorList);

                if (isGood)
                    return new { message = "Game is verified" };
                else
                    return new { errorList = errorList, message = "Game is not verified" };
            }
            catch
            {
                return new { error = "An error occurred while verifying the Game" };
            }
        }

        // DELETE: api/Game/5
        [HttpDelete]
        public object Delete(int id)
        {
            try
            {
                GameProject game;
                using (var db = new NiteLigaContext())
                {
                    game = db.GameProjects.Single(x => x.Id == id);
                    db.GameProjects.Remove(game);
                    db.SaveChanges();
                }
                return new { data = game, message = "Game deleted successfully" };
            }
            catch
            {
                return new { error = "An error occurred while deleting the Game" };
            }
        }

        #region Управление игрой
        // POST: api/Game/5/Start
        [HttpPost]
        public object Start(int id)
        {
            try
            {
                // TODO: Надо запускать игры по gameArchiveId а не по gameProjectId
                // Вероятнее всего надо сделать отдельный api контроллер для работы с GameArchive
                // Будет формироваться PreparedGame, который в свою очередь уже можно будет запускать
                // ... надо обдумать
                ServerGamesManager.StartGame(id, false);
                return new { message = "Game started successfully" };
            }
            catch (Exception ex)
            {
                return new { error = $"Error. {ex.Message}" };
            }
        }

        #endregion
    }
}
