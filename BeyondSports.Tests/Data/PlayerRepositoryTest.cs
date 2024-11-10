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
    public class PlayerRepositoryTests
    {
        private readonly Mock<ILogger<PlayerRepository>> _mockLogger;

        public PlayerRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<PlayerRepository>>();
        }

        private ApplicationDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAllPlayersAsync_ShouldReturnAllPlayers()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new PlayerRepository(context, _mockLogger.Object);

            var team = new Team
            {
                Id = 1,
                Name = "Team1",
                City = "City1",
                Country = "Country1",
                Stadium = "Stadium1"
            };

            var players = new List<Player>
            {
                new Player { Id = 1, Name = "Player1", Team = team },
                new Player { Id = 2, Name = "Player2", Team = team }
            };

            await context.Teams.AddAsync(team);
            await context.Players.AddRangeAsync(players);
            await context.SaveChangesAsync(); // Ensure changes are committed to the in-memory database

            // Verify that players are in the database
            var addedPlayers = await context.Players.ToListAsync();
            Assert.Equal(2, addedPlayers.Count); // Verify that the players were added

            // Act
            var result = await repository.GetAllPlayersAsync();

            // Assert
            Assert.Equal(2, result.Count()); // Ensure the repository returns the correct count of players
        }

        [Fact]
        public async Task GetPlayerByIdAsync_ShouldReturnPlayer_WhenPlayerExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new PlayerRepository(context, _mockLogger.Object);

            var team = new Team
            {
                Id = 1,
                Name = "Team1",
                City = "City1",
                Country = "Country1",
                Stadium = "Stadium1"
            };

            var player = new Player { Id = 1, Name = "Player1", Team = team };

            await context.Teams.AddAsync(team);
            await context.Players.AddAsync(player);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetPlayerByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Player1", result.Name);
        }


        [Fact]
        public async Task GetPlayerByIdAsync_ShouldReturnNull_WhenPlayerDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new PlayerRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.GetPlayerByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddPlayerAsync_ShouldAddPlayer()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new PlayerRepository(context, _mockLogger.Object);
            var player = new Player { Name = "Player1" };

            // Act
            await repository.AddPlayerAsync(player);
            var result = await context.Players.FindAsync(player.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Player1", result.Name);
        }

        [Fact]
        public async Task UpdatePlayerAsync_ShouldUpdatePlayer()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new PlayerRepository(context, _mockLogger.Object);
            var player = new Player { Id = 1, Name = "Player1" };
            await context.Players.AddAsync(player);
            await context.SaveChangesAsync();
            player.Name = "UpdatedPlayer";

            // Act
            await repository.UpdatePlayerAsync(player);
            var result = await context.Players.FindAsync(player.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UpdatedPlayer", result.Name);
        }

        [Fact]
        public async Task DeletePlayerAsync_ShouldDeletePlayer()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new PlayerRepository(context, _mockLogger.Object);
            var player = new Player { Id = 1, Name = "Player1" };
            await context.Players.AddAsync(player);
            await context.SaveChangesAsync();

            // Act
            await repository.DeletePlayerAsync(1);
            var result = await context.Players.FindAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task PlayerExistsInTeamAsync_ShouldReturnTrue_WhenPlayerExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new PlayerRepository(context, _mockLogger.Object);
            var player = new Player { Id = 1, Name = "Player1", TeamId = 1, BirthDate = new DateOnly(2000, 1, 1) };
            await context.Players.AddAsync(player);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.PlayerExistsInTeamAsync("Player1", 1, new DateOnly(2000, 1, 1));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task PlayerExistsInTeamAsync_ShouldReturnFalse_WhenPlayerDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new PlayerRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.PlayerExistsInTeamAsync("Player1", 1, new DateOnly(2000, 1, 1));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task PlayerNumberExistsInTeamAsync_ShouldReturnTrue_WhenNumberExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new PlayerRepository(context, _mockLogger.Object);
            var player = new Player { Id = 1, Name = "Player1", TeamId = 1, Number = 10 };
            await context.Players.AddAsync(player);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.PlayerNumberExistsInTeamAsync(10, 1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task PlayerNumberExistsInTeamAsync_ShouldReturnFalse_WhenNumberDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new PlayerRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.PlayerNumberExistsInTeamAsync(10, 1);

            // Assert
            Assert.False(result);
        }
    }
}
