using NiteLigaLibrary.Database.Models;
using System.Configuration;
using System.Data.Entity;

namespace NiteLigaLibrary.Database
{
    public class NiteLigaContext : DbContext
    {
        /*
                [[Updating]]
                 Enable-Migrations -Force   -Verbose
                 Add-Migration Version_Name -Verbose
                 Update-Database            -Verbose
         */

        public static string ConnectionString = null;
        
        public NiteLigaContext() : base(ConnectionString ?? "DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        public virtual DbSet<StoredGame> StoredGames { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<PlayerInTeam> PlayersInTeams { get; set; }
    }
}