using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Events
{
    /// <summary>
    /// Событие в игре NiteLiga.
    /// </summary>
    public abstract class GameEvent
    {
        /// <summary>
        /// Дата и время события.
        /// </summary>
        public DateTime AddDate { get; set; }
        /// <summary>
        /// Тип события.
        /// </summary>
        public GameEventType Type { get; set; }
        /// <summary>
        /// Запускает обработчик события.
        /// </summary>
        public abstract void Run(GameManager gm);

        public static GameEvent ParseJObject(GameEventType type, JToken o)
        {
            switch (type)
            {
                case GameEventType.AdminSendMessage: return new AdminSendMessage((DateTime)o["AddDate"], (string)o["Message"]);
                case GameEventType.GameAborted: return new GameAborted((DateTime)o["AddDate"]);
                case GameEventType.GameRestored: return new GameRestored((DateTime)o["AddDate"]);
                case GameEventType.GameSaved: return new GameSaved((DateTime)o["AddDate"]);
                case GameEventType.GameStarted: return new GameStarted((DateTime)o["AddDate"]);
                case GameEventType.GameStopped: return new GameStopped((DateTime)o["AddDate"]);
                case GameEventType.PlayerCompletedTask: return new PlayerCompletedTask((DateTime)o["AddDate"], (int)o["TeamId"], (int)o["PlayerId"], (int)o["TaskId"]);
                case GameEventType.PlayerFailedTask: return new PlayerFailedTask((DateTime)o["AddDate"], (int)o["TeamId"], (int)o["PlayerId"], (int)o["TaskId"]);
                case GameEventType.TeamDropTask: return new TeamDropTask((DateTime)o["AddDate"], (int)o["TeamId"], (int)o["TaskIndex"]);
                case GameEventType.TeamGetAddress: return new TeamGetAddress((DateTime)o["AddDate"], (int)o["TeamId"], (int)o["TaskIndex"]);
                case GameEventType.TeamGetHint: return new TeamGetHint((DateTime)o["AddDate"], (int)o["TeamId"], (int)o["TaskIndex"]);
                case GameEventType.TeamStartsPlay: return new TeamStartsPlay((DateTime)o["AddDate"], (int)o["TeamId"]);
                default: throw new Exception($"This GameEventType ({nameof(type)}) not found");
            }
        }
    }
}
