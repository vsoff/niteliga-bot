using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLigaLibrary.Classes
{
    public class GameConfigModel
    {
        public string Address { get; set; }
        public DateTime GameDate { get; set; }
        public string Description { get; set; }
        public List<GameTask> Tasks { get; set; }
        public List<List<int>> TaskGrid { get; set; }

        public bool Verify(out List<string> errors)
        {
            errors = new List<string>();

            if (Tasks.Count < 2)
                errors.Add("Заданий должно быть как минимум два");

            if (TaskGrid.Count < 2)
                errors.Add("Должно быть как минимум 2 сетки");

            foreach (GameTask t in Tasks)
                if (t.Task == "" || t.Code == "" || t.Hint1 == "" || t.Hint2 == "")
                {
                    errors.Add("В каждом задании обязательно должны быть указаны: задание, код, подсказка и слив адреса");
                    break;
                }

            if (TaskGrid.Count > 1)
            {
                int c = TaskGrid[0].Count;
                foreach (List<int> g in TaskGrid)
                {
                    if (g.Count != c)
                    {
                        errors.Add("В каждой сетке должно быть одинаковое количество заданий");
                        break;
                    }
                    if (g.GroupBy(x => x).Where(y => y.Count() > 1).Count() > 0)
                    {
                        errors.Add("В каждой сетке должны быть уникальные задания");
                        break;
                    }
                }
            }

            return errors.Count == 0;
        }
    }
}
