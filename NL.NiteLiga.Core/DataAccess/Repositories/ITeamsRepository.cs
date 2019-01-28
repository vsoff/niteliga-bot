using NL.NiteLiga.Core.DataAccess.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.DataAccess.Repositories
{
    public interface ITeamsRepository
    {
        Team GetTeam(long teamId);
        Team[] GetTeams(long[] teamIds);
        long AddTeam(Team team);
        void DeleteTeam(long teamId);
    }
}
