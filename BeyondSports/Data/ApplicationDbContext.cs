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
                //Twente Enschede FC
                new Player { Id = 1, Name = "Mees Hilgers", Number = 2, Position = Position.Defender, Foot = Foot.Right, BirthDate = new DateOnly(2001, 5, 13), Height = 185, IsInjured = false, TeamId = 1 },
                new Player { Id = 2, Name = "Sem Steijn", Number = 14, Position = Position.Midfielder, Foot = Foot.Right, BirthDate = new DateOnly(2001, 11, 12), Height = 173, IsInjured = false, TeamId = 1 },
                new Player { Id = 3, Name = "Lars Unnerstall", Number = 1, Position = Position.Goalkeeper, Foot = Foot.Right, BirthDate = new DateOnly(1990, 7, 20), Height = 198, IsInjured = false, TeamId = 1 },
                new Player { Id = 4, Name = "Sam Lammers", Number = 10, Position = Position.Forward, Foot = Foot.Both, BirthDate = new DateOnly(1997, 8, 30), Height = 191, IsInjured = false, TeamId = 1 },

                //Ajax Amsterdam
                new Player { Id = 5, Name = "Brian Brobbey", Number = 9, Position = Position.Forward, Foot = Foot.Right, BirthDate = new DateOnly(2002, 2, 2), Height = 182, IsInjured = false, TeamId = 2 },
                new Player { Id = 6, Name = "Jorrel Hato", Number = 4, Position = Position.Defender, Foot = Foot.Left, BirthDate = new DateOnly(2006, 3, 7), Height = 182, IsInjured = false, TeamId = 2 },
                new Player { Id = 7, Name = "Kenneth Taylor", Number = 8, Position = Position.Midfielder, Foot = Foot.Left, BirthDate = new DateOnly(2002, 5, 16), Height = 182, IsInjured = false, TeamId = 2 },
                new Player { Id = 8, Name = "Diant Ramaj", Number = 40, Position = Position.Goalkeeper, Foot = Foot.Both, BirthDate = new DateOnly(2001, 9, 19), Height = 189, IsInjured = false, TeamId = 2 }
            );
        }
    }
}
