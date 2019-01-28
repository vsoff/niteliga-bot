using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameControlCenter.NiteLiga.Game;
using GameControlCenter.NiteLiga.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NL.NiteLiga.Core.DataAccess.Entites;
using NL.NiteLiga.Core.DataAccess.Repositories;

namespace GameControlCenter.Controllers
{
    [Produces("application/json")]
    [Route("api/games")]
    public class GamesController : Controller
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IGameTemplateBuilder _gameTemplateBuilder;

        public GamesController(
            IGamesRepository gamesRepository,
            IGameTemplateBuilder gameTemplateBuilder
            )
        {
            _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
            _gameTemplateBuilder = gameTemplateBuilder ?? throw new ArgumentNullException(nameof(gameTemplateBuilder));
        }

        #region CRUD

        [HttpPost("new")]
        public GameTemplateWebModel Create()
        {
            // Создаём новый шаблон игры.
            GameTemplate template = _gameTemplateBuilder.CreateTemplate();

            // Добавляем шаблон в БД.
            _gamesRepository.AddTemplate(template);

            // Возвращаем новый шаблон в ответ.
            return template.ToWebModel();
        }

        [HttpGet("{id}", Name = "Get")]
        public GameTemplateWebModel Get(long id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public void Update(long id, [FromBody]GameTemplateWebModel model)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        #endregion

        [HttpGet("{id}/verify", Name = "Verify")]
        public object Verify(long id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id}/start")]
        public GameTemplateWebModel Start(long id)
        {
            throw new NotImplementedException();
        }
    }
}
