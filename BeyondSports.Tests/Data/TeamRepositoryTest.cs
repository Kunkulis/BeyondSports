using BeyondSports.Data;
using BeyondSports.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BeyondSports.Tests.Data
{
    public class TeamRepositoryTest
    {
        private readonly Mock<ILogger<TeamRepository>> _mockLogger;

        public TeamRepositoryTest()
        {
            _mockLogger = new Mock<ILogger<TeamRepository>>();
        }

        private ApplicationDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAllTeamsAsync_ShouldReturnAllTeams()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new TeamRepository(context, _mockLogger.Object);

            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Team1", City = "City1", Country = "Country1", Stadium = "Stadium1" },
                new Team { Id = 2, Name = "Team2", City = "City2", Country = "Country2", Stadium = "Stadium2" }
            };

            await context.Teams.AddRangeAsync(teams);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllTeamsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetTeamByIdAsync_ShouldReturnTeam_WhenTeamExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new TeamRepository(context, _mockLogger.Object);

            var team = new Team { Id = 1, Name = "Team1", City = "City1", Country = "Country1", Stadium = "Stadium1" };
            await context.Teams.AddAsync(team);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetTeamByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Team1", result.Name);
        }

        [Fact]
        public async Task GetTeamByIdAsync_ShouldReturnNull_WhenTeamDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new TeamRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.GetTeamByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetTeamByNameAsync_ShouldReturnTeam_WhenTeamExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new TeamRepository(context, _mockLogger.Object);

            var team = new Team { Id = 1, Name = "Team1", City = "City1", Country = "Country1", Stadium = "Stadium1" };
            await context.Teams.AddAsync(team);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetTeamByNameAsync("Team1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Team1", result.Name);
        }

        [Fact]
        public async Task GetTeamByNameAsync_ShouldReturnNull_WhenTeamDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new TeamRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.GetTeamByNameAsync("NonExistentTeam");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateTeamAsync_ShouldAddTeam()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new TeamRepository(context, _mockLogger.Object);

            var team = new Team { Name = "Team1", City = "City1", Country = "Country1", Stadium = "Stadium1" };

            // Act
            await repository.CreateTeamAsync(team);
            var result = await context.Teams.FindAsync(team.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Team1", result.Name);
        }

        [Fact]
        public async Task DeleteTeamAsync_ShouldDeleteTeam()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new TeamRepository(context, _mockLogger.Object);

            var team = new Team { Id = 1, Name = "Team1", City = "City1", Country = "Country1", Stadium = "Stadium1" };
            await context.Teams.AddAsync(team);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteTeamAsync(1);
            var result = await context.Teams.FindAsync(1);

            // Assert
            Assert.Null(result);
        }
    }
}
