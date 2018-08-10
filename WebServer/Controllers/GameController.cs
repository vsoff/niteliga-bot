using NiteLigaLibrary.Database;
using NiteLigaLibrary.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServer.Models;

namespace WebServer.Controllers
{
    public class GameController : ApiController
    {
        // POST: api/Game
        [HttpPost]
        public object Create()
        {
            try
            {
                StoredGame newGame;
                DateTime dt = DateTime.Now;
                using (var db = new NiteLigaContext())
                {
                    newGame = db.StoredGames.Add(new StoredGame
                    {
                        CreateDate = dt,
                        Caption = $"Новая игра ({dt.ToShortDateString()})"
                    });
                    db.SaveChanges();
                }
                return new { data = newGame, message = "New Game added successfully" };
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
                StoredGame game;
                using (var db = new NiteLigaContext())
                {
                    game = db.StoredGames.Single(x => x.Id == id);
                }
                return game;
            }
            catch
            {
                return new { error = "Error while getting Game object" };
            }
        }

        // POST: api/Game/5
        [HttpPost]
        public object Update(int id, StoredGameModel model)
        {
            try
            {
                StoredGame game;
                using (var db = new NiteLigaContext())
                {
                    game = db.StoredGames.Single(x => x.Id == id);
                    game.Caption = model.Caption;
                    game.JSON = model.JSON;
                    db.SaveChanges();
                }
                return new { data = "a", message = "Game deleted successfully" };
            }
            catch
            {
                return new { error = "An error occurred while deleting the Game" };
            }
        }

        // DELETE: api/Game/5
        [HttpDelete]
        public object Delete(int id)
        {
            try
            {
                StoredGame game;
                using (var db = new NiteLigaContext())
                {
                    game = db.StoredGames.Single(x => x.Id == id);
                    db.StoredGames.Remove(game);
                    db.SaveChanges();
                }
                return new { data = game, message = "Game deleted successfully" };
            }
            catch
            {
                return new { error = "An error occurred while deleting the Game" };
            }
        }
    }
}
