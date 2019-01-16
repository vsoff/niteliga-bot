using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NL.NiteLiga.Core.DataAccess.Entites;

namespace NL.NiteLiga.Core.Game.Messengers
{
    public class FakeMessenger : IMessenger
    {
        public void SendMessage(Team team, string message)
        {
            Debug.WriteLine($"Команде {team.Name} [ID {team.Id}] отправлено сообщение: {message}");
        }

        public void SendMessage(Player player, string message)
        {
            Debug.WriteLine($"Игроку {player.GetFullName()} [ID {player.Id}] отправлено сообщение: {message}");
        }

        public void SendMessage(Player[] players, string message)
        {
            Debug.WriteLine($"Игрокам ({players.Length}) IDs: [{string.Join(",", players.Select(x => x.Id))}] отправлено сообщение: {message}");
        }
    }
}
