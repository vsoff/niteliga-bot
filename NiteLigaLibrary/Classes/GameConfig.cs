using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Classes
{
    public class GameConfig : GameConfigModel
    {
        public List<List<GameTask>> Grid { get; set; }

        public GameConfig(string json)
        {
            // Десериализуем JSON
            GameConfigModel gc = JsonConvert.DeserializeObject<GameConfigModel>(json);

            // Заполняем все поля
            this.Address = gc.Address;
            this.GameDate = gc.GameDate;
            this.Description = gc.Description;
            this.Tasks = gc.Tasks;
            this.TaskGrid = gc.TaskGrid;

            // Создаём объект сетки
            Grid = new List<List<GameTask>>();
            foreach (List<int> ids in TaskGrid)
            {
                Grid.Add(new List<GameTask>());
                foreach (int id in ids)
                    Grid[Grid.Count - 1].Add(Tasks[id]);
            }
        }
    }
}
