using Newtonsoft.Json;
using NL.NiteLiga.Core.DataAccess.Entites;
using NL.NiteLiga.Core.DataAccess.Entites.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameControlCenter.NiteLiga.Game
{
    public interface IGameTemplateBuilder
    {
        GameTemplate CreateTemplate();
    }

    public class GameTemplateBuilder : IGameTemplateBuilder
    {
        private const int _tasksCreate = 4;

        public GameTemplate CreateTemplate()
        {
            DateTime currentTime = DateTime.UtcNow;

            GameConfig config = CreateConfig(currentTime);
            GameSettings settings = CreateSettings();

            return new GameTemplate
            {
                Caption = $"Новая игра ({currentTime.ToString("hh:mm dd.MM.yyyy")})",
                JsonConfig = JsonConvert.SerializeObject(config),
                JsonSettings = JsonConvert.SerializeObject(settings),
                CreateDate = currentTime,
                UpdateDate = currentTime
            };
        }

        private GameSettings CreateSettings()
        {
            return new GameSettings
            {
                GameClosingDurationMin = 7,
                GameDurationMin = 5,
                Hint1DelaySec = 15,
                Hint2DelaySec = 30,
                TaskDropDelaySec = 45,
                SecondsDelayStart = 10,
                TeamIds = new long[] { 1, 2, 3, 4 },
            };
        }

        private GameConfig CreateConfig(DateTime currentTime)
        {
            List<GameTask> tasks = new List<GameTask>();
            for (int i = 1; i < _tasksCreate + 1; i++)
                tasks.Add(new GameTask
                {
                    Lat = 0,
                    Lon = 0,
                    Code = i.ToString(),
                    Address = $"Адрес {i}",
                    Hint1 = $"Первая подсказка {i}",
                    Hint2 = $"Вторая подсказка {i}",
                    Task = $"Формулировка задания {i}"
                });

            int[][] taskGridIndexes = tasks.Select((x, i) =>
            {
                return tasks.Select((y, j) =>
                    (i + j) % _tasksCreate
                ).ToArray();
            }).ToArray();


            return new GameConfig
            {
                Description = "Тестовая игра на улице Тьюринга",
                Address = "Ул. Тестовая, д. 32",
                GameDate = currentTime.AddDays(3),
                Tasks = tasks.ToArray(),
                TaskGridIndexes = taskGridIndexes
            };
        }
    }
}
