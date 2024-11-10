using BeyondSports.Controllers;
using BeyondSports.DTOs;
using BeyondSports.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeyondSports.Models;

namespace BeyondSports.Tests.Controllers
{
    public class PlayerControllerTests
    {
        private readonly Mock<IPlayerService> _mockPlayerService;
        private readonly Mock<ILogger<PlayerController>> _mockLogger;
        private readonly PlayerController _controller;

        public PlayerControllerTests()
        {
            _mockPlayerService = new Mock<IPlayerService>();
            _mockLogger = new Mock<ILogger<PlayerController>>();
            _controller = new PlayerController(_mockPlayerService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllPlayers_ShouldReturnOkResult_WithEmptyList_WhenNoPlayersExist()
        {
            // Arrange
            var emptyPlayersList = new List<PlayerDto>();
            _mockPlayerService.Setup(service => service.GetAllPlayersAsync()).ReturnsAsync(emptyPlayersList);

            // Act
            var result = await _controller.GetAllPlayers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PlayerDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        [Fact]
        public async Task GetAllPlayers_ShouldReturnOkResult_WithListOfPlayers()
        {
            // Arrange
            var players = new List<PlayerDto> { new PlayerDto { Id = 1, Name = "Player1" } };
            _mockPlayerService.Setup(service => service.GetAllPlayersAsync()).ReturnsAsync(players);

            // Act
            var result = await _controller.GetAllPlayers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPlayers = Assert.IsAssignableFrom<IEnumerable<PlayerDto>>(okResult.Value);
            Assert.Single(returnedPlayers);
        }

        [Fact]
        public async Task GetAllPlayers_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            _mockPlayerService.Setup(service => service.GetAllPlayersAsync()).ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.GetAllPlayers();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);
            Assert.Equal("An error occurred while getting all players", statusResult.Value);
        }

        [Fact]
        public async Task GetPlayerById_ShouldReturnOkResult_WithPlayer()
        {
            // Arrange
            var player = new PlayerDto { Id = 1, Name = "Player1" };
            _mockPlayerService.Setup(service => service.GetPlayerByIdAsync(1)).ReturnsAsync(player);

            // Act
            var result = await _controller.GetPlayerById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPlayer = Assert.IsType<PlayerDto>(okResult.Value);
            Assert.Equal("Player1", returnedPlayer.Name);
        }

        [Fact]
        public async Task GetPlayerById_ShouldReturnNotFound_WhenPlayerDoesNotExist()
        {
            // Arrange
            _mockPlayerService.Setup(service => service.GetPlayerByIdAsync(1)).ReturnsAsync((PlayerDto)null!);

            // Act
            var result = await _controller.GetPlayerById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Player with Id 1 doesn't exist", notFoundResult.Value);
        }

        [Fact]
        public async Task AddPlayer_ShouldReturnCreatedAtAction_WhenPlayerIsAdded()
        {
            // Arrange
            var newPlayer = new CreatePlayerDto { Name = "Player1" };
            var playerDto = new PlayerDto { Id = 1, Name = "Player1" };
            _mockPlayerService.Setup(service => service.AddPlayerAsync(newPlayer))
                .ReturnsAsync((true, null!, playerDto));

            // Act
            var result = await _controller.AddPlayer(newPlayer);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedPlayer = Assert.IsType<PlayerDto>(createdResult.Value);
            Assert.Equal("Player1", returnedPlayer.Name);
        }

        [Fact]
        public async Task AddPlayer_ShouldReturnBadRequest_WhenPlayerCreationFails()
        {
            // Arrange
            var newPlayer = new CreatePlayerDto { Name = "Player1" };
            _mockPlayerService.Setup(service => service.AddPlayerAsync(newPlayer))
                .ReturnsAsync((false, "Player already exists", null!));

            // Act
            var result = await _controller.AddPlayer(newPlayer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Player already exists", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePlayer_ShouldReturnNoContent_WhenPlayerIsUpdated()
        {
            // Arrange
            var updatePlayer = new UpdatePlayerDto { Position = "Forward" };
            _mockPlayerService.Setup(service => service.UpdatePlayerAsync(1, updatePlayer))
                .ReturnsAsync((true, null!));

            // Act
            var result = await _controller.UpdatePlayer(1, updatePlayer);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdatePlayer_ShouldReturnNotFound_WhenPlayerDoesNotExist()
        {
            // Arrange
            var updatePlayer = new UpdatePlayerDto { Number = 12 };
            _mockPlayerService.Setup(service => service.UpdatePlayerAsync(1, updatePlayer))
                .ReturnsAsync((false, "Player not found"));

            // Act
            var result = await _controller.UpdatePlayer(1, updatePlayer);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Player not found", notFoundResult.Value);
        }

        [Fact]
        public async Task DeletePlayer_ShouldReturnNoContent_WhenPlayerIsDeleted()
        {
            // Arrange
            _mockPlayerService.Setup(service => service.DeletePlayerAsync(1))
                .ReturnsAsync((true, null!));

            // Act
            var result = await _controller.DeletePlayer(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePlayer_ShouldReturnNotFound_WhenPlayerDoesNotExist()
        {
            // Arrange
            _mockPlayerService.Setup(service => service.DeletePlayerAsync(1))
                .ReturnsAsync((false, "Player not found"));

            // Act
            var result = await _controller.DeletePlayer(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Player not found", notFoundResult.Value);
        }

        [Fact]
        public async Task AddPlayer_ShouldReturnFailure_WhenNameIsTooShort()
        {
            // Arrange
            var invalidPlayer = new CreatePlayerDto
            {
                Name = "A",
                Number = 10,
                Position = "Forward",
                Foot = "Left",
                BirthDate = new DateOnly(2000, 1, 1),
                Height = 180,
                TeamId = 1
            };

            _controller.ModelState.AddModelError("Name", "The Name field is required and must be at least 2 characters long.");

            // Act
            var result = await _controller.AddPlayer(invalidPlayer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task AddPlayer_ShouldReturnFailure_WhenNumberIsOutOfRange()
        {
            // Arrange
            var invalidPlayer = new CreatePlayerDto
            {
                Name = "Valid Player",
                Number = 100,
                Position = "Forward",
                Foot = "Left",
                BirthDate = new DateOnly(2000, 1, 1),
                Height = 180,
                TeamId = 1
            };

            _controller.ModelState.AddModelError("Number", "The field Number must be between 1 and 99.");

            // Act
            var result = await _controller.AddPlayer(invalidPlayer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task AddPlayer_ShouldReturnFailure_WhenPositionIsInvalid()
        {
            // Arrange
            var invalidPlayer = new CreatePlayerDto
            {
                Name = "Valid Player",
                Number = 10,
                Position = "InvalidPosition",
                Foot = "Left",
                BirthDate = new DateOnly(2000, 1, 1),
                Height = 180,
                TeamId = 1
            };

            _controller.ModelState.AddModelError("Position", "Invalid value for Position. Allowed values are: Forward, Midfielder, Defender, Goalkeeper");

            // Act
            var result = await _controller.AddPlayer(invalidPlayer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task AddPlayer_ShouldReturnFailure_WhenFootIsInvalid()
        {
            // Arrange
            var invalidPlayer = new CreatePlayerDto
            {
                Name = "Valid Player",
                Number = 10,
                Position = "Forward",
                Foot = "InvalidFoot",
                BirthDate = new DateOnly(2000, 1, 1),
                Height = 180,
                TeamId = 1
            };

            _controller.ModelState.AddModelError("Foot", "Invalid value for Foot. Allowed values are: Left, Right");

            // Act
            var result = await _controller.AddPlayer(invalidPlayer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task AddPlayer_ShouldReturnFailure_WhenAgeIsOutOfRange()
        {
            // Arrange
            var invalidPlayer = new CreatePlayerDto
            {
                Name = "Valid Player",
                Number = 10,
                Position = "Forward",
                Foot = "Left",
                BirthDate = new DateOnly(2010, 1, 1),
                Height = 180,
                TeamId = 1
            };

            _controller.ModelState.AddModelError("BirthDate", "The player's age must be between 16 and 50 years.");

            // Act
            var result = await _controller.AddPlayer(invalidPlayer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task AddPlayer_ShouldReturnSuccess_WhenInputIsValid()
        {
            // Arrange
            var validPlayer = new CreatePlayerDto
            {
                Name = "Valid Player",
                Number = 10,
                Position = "Forward",
                Foot = "Left",
                BirthDate = new DateOnly(2000, 1, 1),
                Height = 180,
                TeamId = 1
            };
            var playerDto = new PlayerDto { Id = 1, Name = "Valid Player" };

            _mockPlayerService.Setup(service => service.AddPlayerAsync(validPlayer))
                .ReturnsAsync((true, null!, playerDto));

            // Act
            var result = await _controller.AddPlayer(validPlayer);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedPlayer = Assert.IsType<PlayerDto>(createdResult.Value);
            Assert.Equal("Valid Player", returnedPlayer.Name);
        }
    }
}
