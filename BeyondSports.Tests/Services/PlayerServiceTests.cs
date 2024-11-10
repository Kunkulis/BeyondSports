using AutoMapper;
using BeyondSports.Data;
using BeyondSports.DTOs;
using BeyondSports.Models;
using BeyondSports.Services;
using BeyondSports.Enum;
using Microsoft.Extensions.Logging;
using Moq;

namespace BeyondSports.Tests.Services
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _mockPlayerRepo;
        private readonly Mock<ITeamRepository> _mockTeamRepo;
        private readonly Mock<ILogger<PlayerService>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PlayerService _playerService;

        public PlayerServiceTests()
        {
            _mockPlayerRepo = new Mock<IPlayerRepository>();
            _mockTeamRepo = new Mock<ITeamRepository>();
            _mockLogger = new Mock<ILogger<PlayerService>>();
            _mockMapper = new Mock<IMapper>();
            _playerService = new PlayerService(_mockPlayerRepo.Object, _mockTeamRepo.Object, _mockLogger.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllPlayersAsync_ShouldReturnListOfPlayers()
        {
            // Arrange
            var players = new List<Player> { new Player { Id = 1, Name = "Player1" } };
            _mockPlayerRepo.Setup(repo => repo.GetAllPlayersAsync()).ReturnsAsync(players);
            _mockMapper.Setup(m => m.Map<List<PlayerDto>>(It.IsAny<List<Player>>())).Returns(new List<PlayerDto> { new PlayerDto { Id = 1, Name = "Player1" } });

            // Act
            var result = await _playerService.GetAllPlayersAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Player1", result.First().Name);
        }

        [Fact]
        public async Task GetPlayerByIdAsync_ShouldReturnPlayer_WhenPlayerExists()
        {
            // Arrange
            var player = new Player { Id = 1, Name = "Player1" };
            _mockPlayerRepo.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync(player);
            _mockMapper.Setup(m => m.Map<PlayerDto>(It.IsAny<Player>())).Returns(new PlayerDto { Id = 1, Name = "Player1" });

            // Act
            var result = await _playerService.GetPlayerByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Player1", result.Name);
        }

        [Fact]
        public async Task GetPlayerByIdAsync_ShouldReturnNull_WhenPlayerDoesNotExist()
        {
            // Arrange
            _mockPlayerRepo.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync((Player)null!);

            // Act
            var result = await _playerService.GetPlayerByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddPlayerAsync_ShouldReturnSuccess_WhenPlayerIsValid()
        {
            // Arrange
            var newPlayer = new CreatePlayerDto { Name = "Player1", TeamId = 1, BirthDate = new DateOnly(2000, 1, 1), Number = 10 };
            var team = new Team { Id = 1, Name = "Team1" };
            var player = new Player { Id = 1, Name = "Player1" };
            var playerDto = new PlayerDto { Id = 1, Name = "Player1" };

            _mockTeamRepo.Setup(repo => repo.GetTeamByIdAsync(1)).ReturnsAsync(team);
            _mockPlayerRepo.Setup(repo => repo.PlayerExistsInTeamAsync(newPlayer.Name, newPlayer.TeamId, newPlayer.BirthDate)).ReturnsAsync(false);
            _mockPlayerRepo.Setup(repo => repo.PlayerNumberExistsInTeamAsync(newPlayer.Number, newPlayer.TeamId)).ReturnsAsync(false);
            _mockMapper.Setup(m => m.Map<Player>(newPlayer)).Returns(player);
            _mockPlayerRepo.Setup(repo => repo.AddPlayerAsync(player)).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<PlayerDto>(player)).Returns(playerDto);

            // Act
            var (success, errorMessage, result) = await _playerService.AddPlayerAsync(newPlayer);

            // Assert
            Assert.True(success);
            Assert.Null(errorMessage);
            Assert.Equal("Player1", result.Name);
        }

        [Fact]
        public async Task AddPlayerAsync_ShouldReturnFailure_WhenPlayerAlreadyExists()
        {
            // Arrange
            var newPlayer = new CreatePlayerDto { Name = "Player1", TeamId = 1, BirthDate = new DateOnly(2000, 1, 1), Number = 10 };
            var team = new Team { Id = 1, Name = "Team1" };

            _mockTeamRepo.Setup(repo => repo.GetTeamByIdAsync(1)).ReturnsAsync(team);
            _mockPlayerRepo.Setup(repo => repo.PlayerExistsInTeamAsync(newPlayer.Name, newPlayer.TeamId, newPlayer.BirthDate)).ReturnsAsync(true);

            // Act
            var (success, errorMessage, result) = await _playerService.AddPlayerAsync(newPlayer);

            // Assert
            Assert.False(success);
            Assert.Equal("A player with this name already exists in the specified team.", errorMessage);
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdatePlayerAsync_ShouldReturnSuccess_WhenPlayerIsUpdated()
        {
            // Arrange
            var updatePlayer = new UpdatePlayerDto { Position = "Forward" };
            var player = new Player { Id = 1, Name = "Player1" };

            _mockPlayerRepo.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync(player);
            _mockMapper.Setup(m => m.Map(updatePlayer, player)).Callback(() => player.Position = Enum.Position.Forward);
            _mockPlayerRepo.Setup(repo => repo.UpdatePlayerAsync(player)).Returns(Task.CompletedTask);

            // Act
            var (success, errorMessage) = await _playerService.UpdatePlayerAsync(1, updatePlayer);

            // Assert
            Assert.True(success);
            Assert.Null(errorMessage);
            Assert.Equal(Position.Forward, player.Position);
        }

        [Fact]
        public async Task DeletePlayerAsync_ShouldReturnSuccess_WhenPlayerIsDeleted()
        {
            // Arrange
            var player = new Player { Id = 1, Name = "Player1" };

            _mockPlayerRepo.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync(player);
            _mockPlayerRepo.Setup(repo => repo.DeletePlayerAsync(1)).Returns(Task.CompletedTask);

            // Act
            var (success, errorMessage) = await _playerService.DeletePlayerAsync(1);

            // Assert
            Assert.True(success);
            Assert.Null(errorMessage);
        }

        [Fact]
        public async Task DeletePlayerAsync_ShouldReturnFailure_WhenPlayerDoesNotExist()
        {
            // Arrange
            _mockPlayerRepo.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync((Player)null!);

            // Act
            var (success, errorMessage) = await _playerService.DeletePlayerAsync(1);

            // Assert
            Assert.False(success);
            Assert.Equal("Player with Id 1 doesn't exist", errorMessage);
        }

        [Fact]
        public async Task AddPlayerAsync_ShouldReturnFailure_WhenTeamDoesNotExist()
        {
            // Arrange
            var newPlayer = new CreatePlayerDto { Name = "Player1", TeamId = 99, BirthDate = new DateOnly(2000, 1, 1), Number = 10 };

            _mockTeamRepo.Setup(repo => repo.GetTeamByIdAsync(99)).ReturnsAsync((Team)null!);

            // Act
            var (success, errorMessage, result) = await _playerService.AddPlayerAsync(newPlayer);

            // Assert
            Assert.False(success);
            Assert.Equal("Team with Id 99 doesn't exist", errorMessage);
            Assert.Null(result);
        }

        [Fact]
        public async Task AddPlayerAsync_ShouldReturnFailure_WhenPlayerNumberAlreadyExistsInTeam()
        {
            // Arrange
            var newPlayer = new CreatePlayerDto { Name = "Player1", TeamId = 1, BirthDate = new DateOnly(2000, 1, 1), Number = 10 };
            var team = new Team { Id = 1, Name = "Team1" };

            _mockTeamRepo.Setup(repo => repo.GetTeamByIdAsync(1)).ReturnsAsync(team);
            _mockPlayerRepo.Setup(repo => repo.PlayerExistsInTeamAsync(newPlayer.Name, newPlayer.TeamId, newPlayer.BirthDate)).ReturnsAsync(false);
            _mockPlayerRepo.Setup(repo => repo.PlayerNumberExistsInTeamAsync(newPlayer.Number, newPlayer.TeamId)).ReturnsAsync(true);

            // Act
            var (success, errorMessage, result) = await _playerService.AddPlayerAsync(newPlayer);

            // Assert
            Assert.False(success);
            Assert.Equal("The number 10 is already assigned to another player in this team.", errorMessage);
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdatePlayerAsync_ShouldReturnFailure_WhenPlayerDoesNotExist()
        {
            // Arrange
            var updatePlayer = new UpdatePlayerDto { Position = "Forward" };

            _mockPlayerRepo.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync((Player)null!);

            // Act
            var (success, errorMessage) = await _playerService.UpdatePlayerAsync(1, updatePlayer);

            // Assert
            Assert.False(success);
            Assert.Equal("Player with ID 1 not found.", errorMessage);
        }

        [Fact]
        public async Task UpdatePlayerAsync_ShouldReturnFailure_WhenNewTeamNameDoesNotExist()
        {
            // Arrange
            var updatePlayer = new UpdatePlayerDto { TeamName = "NonExistentTeam" };
            var player = new Player { Id = 1, Name = "Player1", TeamId = 1 };

            _mockPlayerRepo.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync(player);
            _mockTeamRepo.Setup(repo => repo.GetTeamByNameAsync("NonExistentTeam")).ReturnsAsync((Team)null!);

            // Act
            var (success, errorMessage) = await _playerService.UpdatePlayerAsync(1, updatePlayer);

            // Assert
            Assert.False(success);
            Assert.Equal("Team 'NonExistentTeam' not found.", errorMessage);
        }

        [Fact]
        public async Task UpdatePlayerAsync_ShouldUpdateTeam_WhenNewTeamNameExists()
        {
            // Arrange
            var updatePlayer = new UpdatePlayerDto { TeamName = "NewTeam" };
            var player = new Player { Id = 1, Name = "Player1", TeamId = 1 };
            var newTeam = new Team { Id = 2, Name = "NewTeam" };

            _mockPlayerRepo.Setup(repo => repo.GetPlayerByIdAsync(1)).ReturnsAsync(player);
            _mockTeamRepo.Setup(repo => repo.GetTeamByNameAsync("NewTeam")).ReturnsAsync(newTeam);
            _mockPlayerRepo.Setup(repo => repo.UpdatePlayerAsync(player)).Returns(Task.CompletedTask);

            // Act
            var (success, errorMessage) = await _playerService.UpdatePlayerAsync(1, updatePlayer);

            // Assert
            Assert.True(success);
            Assert.Null(errorMessage);
            Assert.Equal(2, player.TeamId);
        }        
    }
}
