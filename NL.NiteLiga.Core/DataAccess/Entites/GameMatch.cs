using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.DataAccess.Entites
{
    public class GameMatch
    {
        public long Id { get; set; }
        public string JsonLog { get; set; }
        public bool IsTestRun { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public long GameProjectId { get; set; }
    }
}
