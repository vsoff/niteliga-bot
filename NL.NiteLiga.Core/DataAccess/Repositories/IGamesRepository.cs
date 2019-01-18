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
        GameMatch[] GetMatches(long gameTemplateId);
    }
}
