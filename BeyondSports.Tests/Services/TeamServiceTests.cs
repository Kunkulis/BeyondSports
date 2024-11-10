using AutoMapper;
using BeyondSports.Data;
using BeyondSports.DTOs;
using BeyondSports.Models;
using BeyondSports.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BeyondSports.Tests.Services
{
    public class TeamServiceTests
    {
        private readonly Mock<ITeamRepository> _mockTeamRepo;
        private readonly Mock<ILogger<TeamService>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TeamService _teamService;

        public TeamServiceTests()
        {
            _mockTeamRepo = new Mock<ITeamRepository>();
            _mockLogger = new Mock<ILogger<TeamService>>();
            _mockMapper = new Mock<IMapper>();
            _teamService = new TeamService(_mockTeamRepo.Object, _mockLogger.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllTeamsAsync_ShouldReturnListOfTeams()
        {
            // Arrange
            var teams = new List<Team> { new Team { Id = 1, Name = "Team1" } };
            _mockTeamRepo.Setup(repo => repo.GetAllTeamsAsync()).ReturnsAsync(teams);
            _mockMapper.Setup(m => m.Map<List<TeamDto>>(teams)).Returns(new List<TeamDto> { new TeamDto { Id = 1, Name = "Team1" } });

            // Act
            var result = await _teamService.GetAllTeamsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Team1", result.First().Name);
        }

        [Fact]
        public async Task GetTeamByIdAsync_ShouldReturnTeam_WhenTeamExists()
        {
            // Arrange
            var team = new Team { Id = 1, Name = "Team1" };
            _mockTeamRepo.Setup(repo => repo.GetTeamByIdAsync(1)).ReturnsAsync(team);
            _mockMapper.Setup(m => m.Map<TeamDto>(team)).Returns(new TeamDto { Id = 1, Name = "Team1" });

            // Act
            var result = await _teamService.GetTeamByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Team1", result.Name);
        }

        [Fact]
        public async Task GetTeamByIdAsync_ShouldReturnNull_WhenTeamDoesNotExist()
        {
            // Arrange
            _mockTeamRepo.Setup(repo => repo.GetTeamByIdAsync(1)).ReturnsAsync((Team)null!);

            // Act
            var result = await _teamService.GetTeamByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateTeamAsync_ShouldReturnSuccess_WhenTeamIsValid()
        {
            // Arrange
            var newTeam = new CreateTeamDto { Name = "Team1" };
            var team = new Team { Id = 1, Name = "Team1" };
            var teamDto = new TeamDto { Id = 1, Name = "Team1" };

            _mockTeamRepo.Setup(repo => repo.GetTeamByNameAsync(newTeam.Name)).ReturnsAsync((Team)null!);
            _mockMapper.Setup(m => m.Map<Team>(newTeam)).Returns(team);
            _mockTeamRepo.Setup(repo => repo.CreateTeamAsync(team)).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<TeamDto>(team)).Returns(teamDto);

            // Act
            var (success, errorMessage, result) = await _teamService.CreateTeamAsync(newTeam);

            // Assert
            Assert.True(success);
            Assert.Null(errorMessage);
            Assert.Equal("Team1", result.Name);
        }

        [Fact]
        public async Task CreateTeamAsync_ShouldReturnFailure_WhenTeamAlreadyExists()
        {
            // Arrange
            var newTeam = new CreateTeamDto { Name = "Team1" };
            var existingTeam = new Team { Id = 1, Name = "Team1" };

            _mockTeamRepo.Setup(repo => repo.GetTeamByNameAsync(newTeam.Name)).ReturnsAsync(existingTeam);

            // Act
            var (success, errorMessage, result) = await _teamService.CreateTeamAsync(newTeam);

            // Assert
            Assert.False(success);
            Assert.Equal("A team with this name already exists.", errorMessage);
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteTeamAsync_ShouldReturnSuccess_WhenTeamIsDeleted()
        {
            // Arrange
            var team = new Team { Id = 1, Name = "Team1" };
            _mockTeamRepo.Setup(repo => repo.GetTeamByIdAsync(1)).ReturnsAsync(team);
            _mockTeamRepo.Setup(repo => repo.DeleteTeamAsync(1)).Returns(Task.CompletedTask);

            // Act
            var (success, errorMessage) = await _teamService.DeleteTeamAsync(1);

            // Assert
            Assert.True(success);
            Assert.Null(errorMessage);
        }

        [Fact]
        public async Task DeleteTeamAsync_ShouldReturnFailure_WhenTeamDoesNotExist()
        {
            // Arrange
            _mockTeamRepo.Setup(repo => repo.GetTeamByIdAsync(1)).ReturnsAsync((Team)null!);

            // Act
            var (success, errorMessage) = await _teamService.DeleteTeamAsync(1);

            // Assert
            Assert.False(success);
            Assert.Equal("Team not found.", errorMessage);
        }       
    }
}