﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NL.NiteLiga.Core.DataAccess.DbContexts;
using NL.NiteLiga.Core.DataAccess.Entites;

namespace NL.NiteLiga.Core.DataAccess.Repositories
{
    public class TeamsRepository : ITeamsRepository
    {
        private readonly MembershipStatus[] _inTeamMembershipStatuses = new[] { MembershipStatus.Captain, MembershipStatus.Player, MembershipStatus.Legionary };

        private readonly INiteLigaContextProvider _contextProvider;

        public TeamsRepository(INiteLigaContextProvider contextProvider)
        {
            _contextProvider = contextProvider ?? throw new ArgumentNullException(nameof(contextProvider));
        }

        public Team GetTeam(long teamId)
        {
            using (var context = _contextProvider.GetContext())
            {
                var team = context.Teams.First(x => x.Id == teamId);
                team.Players = context.Memberships.Where(x => x.TeamId == teamId).Select(x => x.Player).ToArray();
                return team;
            }
        }

        public Team[] GetTeams(long[] teamIds)
        {
            using (var context = _contextProvider.GetContext())
            {
                var teams = context.Teams.Where(x => teamIds.Contains(x.Id));
                var memberships = context.Memberships.Where(
                    x => teamIds.Contains(x.TeamId)
                    && x.LeaveDate == null
                    && _inTeamMembershipStatuses.Contains(x.Status)
                ).ToArray();
                foreach (var team in teams)
                    team.Players = memberships.Where(x => x.TeamId == team.Id).Select(x => x.Player).ToArray();
                return teams.ToArray();
            }
        }
    }
}
