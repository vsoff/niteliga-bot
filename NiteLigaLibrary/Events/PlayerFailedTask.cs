using NiteLigaLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Events
{
    public class PlayerFailedTask : GameEvent
    {
        public int PlayerId { get; set; }
        public int TaskIndex { get; set; }
        public int TeamId { get; set; }

        public PlayerFailedTask(DateTime date, int teamId, int playerId, int taskId)
        {
            this.AddDate = date;
            this.PlayerId = playerId;
            this.TaskIndex = taskId;
            this.TeamId = teamId;
            this.Type = GameEventType.PlayerFailedTask;
        }

        public override void Run(GameManager gm)
        {
            gm.Noticer.AddOutputMessage(new Message(
                gm.Teams.First(x => x.Id == TeamId).Players.First(x => x.Id == PlayerId),
                "Неправильный ответ на задание."
                ));
        }
    }
}
