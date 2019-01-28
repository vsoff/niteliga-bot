using NL.NiteLiga.Core.DataAccess.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.DataAccess.Repositories
{
    public interface IGamesRepository
    {
        GameTemplate[] GetAllTemplatesLight();
        GameTemplate GetTemplateHard(long gameTemplateId);
        long AddTemplate(GameTemplate template);
        void DeleteTemplate(long templateId);

        GameMatch[] GetMatches(long gameTemplateId);
        long AddMatch(GameMatch match);
        void DeleteMatch(long matchId);
    }
}
