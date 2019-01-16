using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NL.NiteLiga.Core.DataAccess.DbContexts;
using NL.NiteLiga.Core.DataAccess.Entites;

namespace NL.NiteLiga.Core.DataAccess.Repositories
{
    public class GamesRepository : IGamesRepository
    {
        private readonly INiteLigaContextProvider _contextProvider;

        public GamesRepository(INiteLigaContextProvider contextProvider)
        {
            _contextProvider = contextProvider ?? throw new ArgumentNullException(nameof(contextProvider));
        }

        public GameMatch[] GetMatches(long gameTemplateId)
        {
            using (var context = _contextProvider.GetContext())
            {
                var gameMatches = context.GameMatches.Where(x => x.GameProjectId == gameTemplateId);
                return gameMatches.ToArray();
            }
        }

        public GameTemplate GetTemplate(long gameTemplateId)
        {
            using (var context = _contextProvider.GetContext())
            {
                var gameProject = context.GameTemplates.First(x => x.Id == gameTemplateId);
                return gameProject;
            }
        }
    }
}
