using NL.NiteLiga.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Repositories
{
    public interface IGamesRepository
    {
        GameProject GetProject(long gameProjectId);
        GameArchive[] GetArchives(long gameProjectId);
    }
}
