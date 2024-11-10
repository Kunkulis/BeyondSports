using Microsoft.EntityFrameworkCore;
using BeyondSports.Models;
using BeyondSports.Enum;

namespace BeyondSports.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>().HasData(
                new Team { Id = 1, Name = "Twente Enschede FC", Country = "The Netherlands", City = "Enschede", Stadium = "De Grolsch Veste" },
                new Team { Id = 2, Name = "Ajax Amsterdam", Country = "The Netherlands", City = "Amsterdam", Stadium = "Johan Cruyff Arena" }
            );

            modelBuilder.Entity<Player>().HasData(
                new Player { Id = 1, Name = "Player1", Number = 1, Position = Position.Goalkeeper, Foot = Foot.Right, BirthDate = new DateOnly(1993, 1, 1), Height = 185, IsInjured = false, TeamId = 1 },
                new Player { Id = 2, Name = "Player2", Number = 2, Position = Position.Defender, Foot = Foot.Left, BirthDate = new DateOnly(1995, 2, 1), Height = 180, IsInjured = false, TeamId = 2 },
                new Player { Id = 3, Name = "Player3", Number = 3, Position = Position.Defender, Foot = Foot.Right, BirthDate = new DateOnly(1998, 3, 1), Height = 182, IsInjured = false, TeamId = 1 },
                new Player { Id = 4, Name = "Player4", Number = 4, Position = Position.Midfielder, Foot = Foot.Left, BirthDate = new DateOnly(1996, 4, 1), Height = 178, IsInjured = false, TeamId = 2 },
                new Player { Id = 5, Name = "Player5", Number = 5, Position = Position.Midfielder, Foot = Foot.Right, BirthDate = new DateOnly(1999, 5, 1), Height = 176, IsInjured = false, TeamId = 1 },
                new Player { Id = 6, Name = "Player6", Number = 6, Position = Position.Forward, Foot = Foot.Left, BirthDate = new DateOnly(1994, 6, 1), Height = 183, IsInjured = false, TeamId = 2 },
                new Player { Id = 7, Name = "Player7", Number = 7, Position = Position.Forward, Foot = Foot.Right, BirthDate = new DateOnly(1997, 7, 1), Height = 180, IsInjured = false, TeamId = 1 },
                new Player { Id = 8, Name = "Player8", Number = 8, Position = Position.Goalkeeper, Foot = Foot.Left, BirthDate = new DateOnly(1992, 8, 1), Height = 190, IsInjured = false, TeamId = 2 },
                new Player { Id = 9, Name = "Player9", Number = 9, Position = Position.Defender, Foot = Foot.Right, BirthDate = new DateOnly(2001, 9, 1), Height = 175, IsInjured = false, TeamId = 1 },
                new Player { Id = 10, Name = "Player10", Number = 10, Position = Position.Midfielder, Foot = Foot.Left, BirthDate = new DateOnly(2000, 10, 1), Height = 177, IsInjured = false, TeamId = 2 },
                new Player { Id = 11, Name = "Player11", Number = 11, Position = Position.Forward, Foot = Foot.Right, BirthDate = new DateOnly(1995, 11, 1), Height = 181, IsInjured = false, TeamId = 1 },
                new Player { Id = 12, Name = "Player12", Number = 12, Position = Position.Goalkeeper, Foot = Foot.Right, BirthDate = new DateOnly(1993, 12, 1), Height = 185, IsInjured = false, TeamId = 2 },
                new Player { Id = 13, Name = "Player13", Number = 13, Position = Position.Defender, Foot = Foot.Left, BirthDate = new DateOnly(1996, 1, 1), Height = 180, IsInjured = false, TeamId = 1 },
                new Player { Id = 14, Name = "Player14", Number = 14, Position = Position.Midfielder, Foot = Foot.Right, BirthDate = new DateOnly(1998, 2, 1), Height = 182, IsInjured = false, TeamId = 2 }
            );
        }
    }
}
