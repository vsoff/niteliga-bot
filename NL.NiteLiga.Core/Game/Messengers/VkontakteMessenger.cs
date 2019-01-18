using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NL.NiteLiga.Core.DataAccess.Entites;

namespace NL.NiteLiga.Core.Game.Messengers
{
    public class VkontakteMessenger : IMessenger
    {
        public void SendMessage(Team team, string message)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(Player player, string message)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(Player[] players, string message)
        {
            throw new NotImplementedException();
        }
    }
}
