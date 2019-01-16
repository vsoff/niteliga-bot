using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Game.Messengers
{
    public class MessagePool : IMessagePool
    {
        /// <summary>
        /// Очередь сообщений.
        /// </summary>
        private readonly ConcurrentDictionary<long, ConcurrentQueue<Message>> _gameQueues;

        public MessagePool()
        {
            _gameQueues = new ConcurrentDictionary<long, ConcurrentQueue<Message>>();
        }

        public void NewQueue(long gameMatchId)
        {
            _gameQueues[gameMatchId] = new ConcurrentQueue<Message>();
        }

        public void DeleteQueue(long gameMatchId)
        {
            ConcurrentQueue<Message> temp;
            _gameQueues.TryRemove(gameMatchId, out temp);
        }

        public void AddMessage(Message message)
        {
            _gameQueues[message.GameMatchId].Enqueue(message);
        }

        public Message[] GetMessages(long gameMatchId)
        {
            List<Message> messages = new List<Message>(_gameQueues[gameMatchId].Count);

            while (_gameQueues[gameMatchId].Count > 0)
            {
                Message message;
                _gameQueues[gameMatchId].TryDequeue(out message);
                messages.Add(message);
            }

            return messages.ToArray();
        }
    }
}
