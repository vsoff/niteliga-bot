using NL.NiteLiga.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Messengers
{
    interface IMessager
    {
        void SendMessage(Team team, string message);
        void SendMessage(Player player, string message);
        void SendMessage(Player[] players, string message);
    }
}
