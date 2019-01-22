using NiteLigaLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Events
{
    public class TeamStartsPlay : GameEvent
    {
        public int TeamId { get; set; }

        public TeamStartsPlay(DateTime date, int teamId)
        {
            this.AddDate = date;
            this.Type = GameEventType.TeamStartsPlay;
            this.TeamId = teamId;
        }

        public override void Run(GameManager gm)
        {
            LocalTeam team = gm.Teams.First(x => x.Id == TeamId);
            team.Progress = new TeamGameProgress(gm.Config.Grid[gm.Teams.IndexOf(team)], AddDate);
            team.SendMessage(gm.Noticer, $"Ваше первое задание: {team.Progress.GetCurrentTask().Task}");
        }
    }
}
