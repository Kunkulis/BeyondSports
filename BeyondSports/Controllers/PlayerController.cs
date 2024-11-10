using BeyondSports.DTOs;
using BeyondSports.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeyondSports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _service;
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(IPlayerService service, ILogger<PlayerController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllPlayers")]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAllPlayers()
        {
            try
            {
                var players = await _service.GetAllPlayersAsync();
                return Ok(players);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all players");
                return StatusCode(500, "An error occurred while getting all players");
            }
        }

        [HttpGet]
        [Route("GetPlayer/{id}")]
        public async Task<ActionResult<PlayerDto>> GetPlayerById(int id)
        {
            try
            {
                var player = await _service.GetPlayerByIdAsync(id);
                if (player == null) return NotFound($"Player with Id {id} doesn't exist");
                return Ok(player);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting player with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("AddPlayer")]
        public async Task<ActionResult> AddPlayer([FromBody] CreatePlayerDto newPlayer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var (success, errorMessage, playerDto) = await _service.AddPlayerAsync(newPlayer);
                if (!success) return BadRequest(errorMessage);

                return CreatedAtAction(nameof(GetPlayerById), new { id = playerDto.Id }, playerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new player.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("UpdatePlayer/{id}")]
        public async Task<ActionResult> UpdatePlayer(int id, [FromBody] UpdatePlayerDto updatePlayer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var (success, errorMessage) = await _service.UpdatePlayerAsync(id, updatePlayer);
                if (!success) return NotFound(errorMessage);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating player with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        [Route("DeletePlayer/{id}")]
        public async Task<ActionResult> DeletePlayer(int id)
        {
            try
            {
                var (success, errorMessage) = await _service.DeletePlayerAsync(id);
                if (!success) return NotFound(errorMessage);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting player with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}