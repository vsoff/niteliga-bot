using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NiteLigaLibrary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Classes
{
    public class GameBackupConverter : JsonCreationConverter<GameBackupModel>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected override GameBackupModel Create(Type objectType, JObject jObject)
        {
            List<GameEvent> gameEvents = new List<GameEvent>();

            foreach (JToken a in jObject["StoredEvents"])
            {
                GameEventType type = (GameEventType)((dynamic)a).Type;
                gameEvents.Add(GameEvent.ParseJObject(type, a));
            }

            return new GameBackupModel
            {
                LaunchTime = (DateTime?)((dynamic)jObject).LaunchTime,
                EndTime = (DateTime?)((dynamic)jObject).EndTime,
                GameStatus = (GameStatusType)((dynamic)jObject).GameStatus,
                TeamProgress = JObject.FromObject(jObject["TeamProgress"]).ToObject<Dictionary<int, TeamProgressBackupModel>>(),
                StoredEvents = gameEvents
            };
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        protected bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            //serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
