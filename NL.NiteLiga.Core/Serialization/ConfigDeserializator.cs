using Newtonsoft.Json;
using NL.NiteLiga.Core.DataAccess.Entites.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.Serialization
{
    public interface INiteLigaDeserializator
    {
        GameConfig SerializeConfig(string jsonConfig);
        GameSettings SerializeSettings(string jsonSettings);
    }

    public class NiteLigaDeserializator : INiteLigaDeserializator
    {
        public GameConfig SerializeConfig(string jsonConfig)
        {
            GameConfig config = JsonConvert.DeserializeObject<GameConfig>(jsonConfig);

            config.TaskGrid = config.TaskGridIndexes.Select(
                    line => line.Select(
                        col => config.Tasks[col]
                    ).ToArray()
                ).ToArray();

            return config;
        }

        public GameSettings SerializeSettings(string jsonSettings)
        {
            return JsonConvert.DeserializeObject<GameSettings>(jsonSettings);
        }
    }
}
