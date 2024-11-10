using BeyondSports.Controllers;
using BeyondSports.DTOs;
using BeyondSports.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BeyondSports.Tests.Controllers
{
    public class TeamControllerTests
    {
        private readonly Mock<ITeamService> _mockTeamService;
        private readonly Mock<ILogger<TeamController>> _mockLogger;
        private readonly TeamController _controller;

        public TeamControllerTests()
        {
            _mockTeamService = new Mock<ITeamService>();
            _mockLogger = new Mock<ILogger<TeamController>>();
            _controller = new TeamController(_mockTeamService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllTeams_ShouldReturnOkResult_WithListOfTeams()
        {
            // Arrange
            var teams = new List<TeamDto> { new TeamDto { Id = 1, Name = "Team1" } };
            _mockTeamService.Setup(service => service.GetAllTeamsAsync()).ReturnsAsync(teams);

            // Act
            var result = await _controller.GetAllTeams();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TeamDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetAllTeams_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            _mockTeamService.Setup(service => service.GetAllTeamsAsync()).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetAllTeams();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetAllTeams_ShouldReturnOkResult_WithEmptyList_WhenNoTeamsExist()
        {
            // Arrange
            var emptyTeamsList = new List<TeamDto>();
            _mockTeamService.Setup(service => service.GetAllTeamsAsync()).ReturnsAsync(emptyTeamsList);

            // Act
            var result = await _controller.GetAllTeams();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TeamDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        [Fact]
        public async Task GetTeamById_ShouldReturnOkResult_WithTeam()
        {
            // Arrange
            var team = new TeamDto { Id = 1, Name = "Team1" };
            _mockTeamService.Setup(service => service.GetTeamByIdAsync(1)).ReturnsAsync(team);

            // Act
            var result = await _controller.GetTeamById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<TeamDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetTeamById_ShouldReturnNotFound_WhenTeamDoesNotExist()
        {
            // Arrange
            _mockTeamService.Setup(service => service.GetTeamByIdAsync(1)).ReturnsAsync((TeamDto)null!);

            // Act
            var result = await _controller.GetTeamById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Team with Id 1 doesn't exist", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateTeam_ShouldReturnCreatedAtAction_WhenTeamIsCreated()
        {
            // Arrange
            var newTeam = new CreateTeamDto { Name = "Team1" };
            var teamDto = new TeamDto { Id = 1, Name = "Team1" };
            _mockTeamService.Setup(service => service.CreateTeamAsync(newTeam)).ReturnsAsync((true, null!, teamDto));

            // Act
            var result = await _controller.CreateTeam(newTeam);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<TeamDto>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task CreateTeam_ShouldReturnBadRequest_WhenTeamCreationFails()
        {
            // Arrange
            var newTeam = new CreateTeamDto { Name = "Team1" };
            _mockTeamService.Setup(service => service.CreateTeamAsync(newTeam)).ReturnsAsync((false, "Error creating team", null!));

            // Act
            var result = await _controller.CreateTeam(newTeam);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error creating team", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteTeam_ShouldReturnNoContent_WhenTeamIsDeleted()
        {
            // Arrange
            _mockTeamService.Setup(service => service.DeleteTeamAsync(1)).ReturnsAsync((true, null!));

            // Act
            var result = await _controller.DeleteTeam(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTeam_ShouldReturnNotFound_WhenTeamDoesNotExist()
        {
            // Arrange
            _mockTeamService.Setup(service => service.DeleteTeamAsync(1)).ReturnsAsync((false, "Team not found"));

            // Act
            var result = await _controller.DeleteTeam(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Team not found", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateTeam_ShouldReturnBadRequest_WhenCountryIsInvalid()
        {
            // Arrange
            var invalidTeam = new CreateTeamDto { Name = "Team1", Country = "InvalidCountry", City = "Amsterdam", Stadium = "Wembley" };
            _controller.ModelState.AddModelError("Country", "Invalid country.");

            // Act
            var result = await _controller.CreateTeam(invalidTeam);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task CreateTeam_ShouldReturnBadRequest_WhenCityIsInvalid()
        {
            // Arrange
            var invalidTeam = new CreateTeamDto { Name = "Team1", Country = "United States", City = "InvalidCity", Stadium = "Wembley" };
            _controller.ModelState.AddModelError("City", "Invalid city.");

            // Act
            var result = await _controller.CreateTeam(invalidTeam);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task CreateTeam_ShouldReturnBadRequest_WhenStadiumIsInvalid()
        {
            // Arrange
            var invalidTeam = new CreateTeamDto { Name = "Team1", Country = "United States", City = "New York", Stadium = "InvalidStadium" };
            _controller.ModelState.AddModelError("Stadium", "Invalid stadium.");

            // Act
            var result = await _controller.CreateTeam(invalidTeam);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

    }
}
