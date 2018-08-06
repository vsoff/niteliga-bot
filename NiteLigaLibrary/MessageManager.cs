using NiteLigaLibrary.Classes;
using NiteLigaLibrary.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary
{
    /// <summary>
    /// Менеджер управления сообщениями.
    /// </summary>
    public class MessageManager
    {
        private List<Message> InputMessagesQueue { get; set; }
        private List<Message> OutputMessagesQueue { get; set; }

        public MessageManager()
        {
            this.InputMessagesQueue = new List<Message>();
            this.OutputMessagesQueue = new List<Message>();
        }

        /// <summary>
        /// Добавляет одно сообщение в очередь полученных сообщений.
        /// </summary>
        public void AddInputMessage(Message message)
        {
            InputMessagesQueue.Add(message);
        }

        /// <summary>
        /// Добавляет в очередь рассылки несколько сообщений.
        /// </summary>
        /// <param name="receivers">Список игроков-получателей.</param>
        /// <param name="text">Текст сообщения.</param>
        public void AddOutputMessages(List<LocalPlayer> receivers, string text)
        {
            foreach (LocalPlayer receiver in receivers)
                OutputMessagesQueue.Add(new Message(receiver, text));
        }

        /// <summary>
        /// Добавляет в очередь рассылки одно сообщение.
        /// </summary>
        public void AddOutputMessage(Message message)
        {
            OutputMessagesQueue.Add(message);
        }

        /// <summary>
        /// Отправляет порцию исходящих сообщений из очереди.
        /// </summary>
        public void Send()
        {
            // TODO: Сделать отсылку порции сообщений.
            throw new Exception("Method isn't yet implemented.");
        }

        /// <summary>
        /// Забирает порцию исходящих сообщений из очереди (с удалением).
        /// </summary>
        public List<Message> PullOutput()
        {
            int msgCount = OutputMessagesQueue.Count;
            List<Message> messages = OutputMessagesQueue.GetRange(0, msgCount).OrderBy(x => x.AddDate).ToList();
            OutputMessagesQueue.RemoveRange(0, msgCount);
            return messages;
        }

        /// <summary>
        /// Забирает порцию входящих сообщений из очереди (с удалением).
        /// </summary>
        public List<Message> PullInput()
        {
            int msgCount = InputMessagesQueue.Count;
            List<Message> messages = InputMessagesQueue.GetRange(0, msgCount).OrderBy(x => x.AddDate).ToList();
            InputMessagesQueue.RemoveRange(0, msgCount);
            return messages;
        }
    }
}
