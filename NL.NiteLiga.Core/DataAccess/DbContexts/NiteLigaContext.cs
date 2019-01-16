using NL.NiteLiga.Core.DataAccess.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.NiteLiga.Core.DataAccess.DbContexts
{
    public class NiteLigaContext : DbContext
    {
        /*
                [[Updating]]
                 Enable-Migrations -Force   -Verbose
                 Add-Migration Version_Name -Verbose
                 Update-Database            -Verbose
         */

        public NiteLigaContext(string connectionString) : base(connectionString)
        {
            //Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<GameMatch> GameMatches { get; set; }
        public virtual DbSet<GameTemplate> GameTemplates { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
    }
}
