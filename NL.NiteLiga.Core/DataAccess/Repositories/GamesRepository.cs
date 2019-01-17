using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NL.NiteLiga.Core.DataAccess.DbContexts;
using NL.NiteLiga.Core.DataAccess.Entites;
using NL.NiteLiga.Core.Serialization;

namespace NL.NiteLiga.Core.DataAccess.Repositories
{
    public class GamesRepository : IGamesRepository
    {
        private readonly INiteLigaContextProvider _contextProvider;
        private readonly INiteLigaDeserializator _niteLigaDeserializator;

        public GamesRepository(
            INiteLigaContextProvider contextProvider,
            INiteLigaDeserializator niteLigaSerializator)
        {
            _contextProvider = contextProvider ?? throw new ArgumentNullException(nameof(contextProvider));
            _niteLigaDeserializator = niteLigaSerializator ?? throw new ArgumentNullException(nameof(niteLigaSerializator));
        }

        public GameTemplate[] GetAllTemplatesLight()
        {
            using (var context = _contextProvider.GetContext())
            {
                return context.GameTemplates.ToArray();
            }
        }

        public GameMatch[] GetMatches(long gameTemplateId)
        {
            using (var context = _contextProvider.GetContext())
            {
                var gameMatches = context.GameMatches.Where(x => x.GameProjectId == gameTemplateId);
                return gameMatches.ToArray();
            }
        }

        public GameTemplate GetTemplateHard(long gameTemplateId)
        {
            using (var context = _contextProvider.GetContext())
            {
                var gameProject = context.GameTemplates.First(x => x.Id == gameTemplateId);

                gameProject.Config = _niteLigaDeserializator.SerializeConfig(gameProject.JsonConfig);
                gameProject.Settings = _niteLigaDeserializator.SerializeSettings(gameProject.JsonSettings);

                return gameProject;
            }
        }
    }
}
