using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Game.Messengers
{
    public interface IMessagePool
    {
        void NewQueue(long gameMatchId);
        void DeleteQueue(long gameMatchId);
        void AddMessage(Message message);
        Message[] GetMessages(long gameMatchId);
    }
}
