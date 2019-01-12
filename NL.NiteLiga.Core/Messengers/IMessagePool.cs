﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Messengers
{
    interface IMessengePool
    {
        void AddMessage(Message message);
        Message[] GetMessages(long gameId);
    }
}
