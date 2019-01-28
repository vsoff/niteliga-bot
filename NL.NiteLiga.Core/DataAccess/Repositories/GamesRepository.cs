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

        public long AddMatch(GameMatch match)
        {
            using (var context = _contextProvider.GetContext())
            {
                context.GameMatches.Add(match);
                context.SaveChanges();
            }

            return match.Id;
        }

        public long AddTemplate(GameTemplate template)
        {
            using (var context = _contextProvider.GetContext())
            {
                context.GameTemplates.Add(template);
                context.SaveChanges();
            }

            return template.Id;
        }

        public void DeleteMatch(long matchId)
        {
            using (var context = _contextProvider.GetContext())
            {
                var match = context.GameMatches.First(x => x.Id == matchId);
                context.GameMatches.Remove(match);
                context.SaveChanges();
            }
        }

        public void DeleteTemplate(long templateId)
        {
            using (var context = _contextProvider.GetContext())
            {
                var template = context.GameTemplates.First(x => x.Id == templateId);
                context.GameTemplates.Remove(template);
                context.SaveChanges();
            }
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
                var gameMatches = context.GameMatches.Where(x => x.GameTemplateId == gameTemplateId);
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
