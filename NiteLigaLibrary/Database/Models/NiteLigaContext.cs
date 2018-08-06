using System.Data.Entity;

namespace NiteLigaLibrary.Database.Models
{
    public class NiteLigaContext : DbContext
    {
        /*
         [[Updating]]
         Enable-Migrations -Force   -ConnectionString "data source=DESKTOP-10P39AB;initial catalog=NiteLiga;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" -ConnectionProviderName "System.Data.SqlClient" -Verbose
         Add-Migration Version_Name -ConnectionString "data source=DESKTOP-10P39AB;initial catalog=NiteLiga;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" -ConnectionProviderName "System.Data.SqlClient" -Verbose
         Update-Database            -ConnectionString "data source=DESKTOP-10P39AB;initial catalog=NiteLiga;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" -ConnectionProviderName "System.Data.SqlClient" -Verbose
        
         // ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString

         */

        public static string ConnectionString = "";
        
        public NiteLigaContext() : base(ConnectionString)
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