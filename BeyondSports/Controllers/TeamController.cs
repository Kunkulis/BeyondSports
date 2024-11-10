using BeyondSports.DTOs;
using BeyondSports.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeyondSports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _service;
        private readonly ILogger<TeamController> _logger;

        public TeamController(ITeamService service, ILogger<TeamController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllTeams")]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetAllTeams()
        {
            try
            {
                var teams = await _service.GetAllTeamsAsync();
                return Ok(teams);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all teams");
                return StatusCode(500, "An error occurred while getting all teams");
            }
        }

        [HttpGet]
        [Route("GetTeam/{id}")]
        public async Task<ActionResult<TeamDto>> GetTeamById(int id)
        {
            try
            {
                var team = await _service.GetTeamByIdAsync(id);
                if (team == null) return NotFound($"Team with Id {id} doesn't exist");

                return Ok(team);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting team with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("CreateTeam")]
        public async Task<ActionResult> CreateTeam([FromBody] CreateTeamDto newTeam)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var (success, errorMessage, teamDto) = await _service.CreateTeamAsync(newTeam);
                if (!success) return BadRequest(errorMessage);

                return CreatedAtAction(nameof(GetTeamById), new { id = teamDto.Id }, teamDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a team");
                return StatusCode(500, "An error occurred while creating a team");
            }
        }

        [HttpDelete]
        [Route("DeleteTeam/{id}")]
        public async Task<ActionResult> DeleteTeam(int id)
        {
            try
            {
                var (success, errorMessage) = await _service.DeleteTeamAsync(id);
                if (!success) return NotFound(errorMessage);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a team");
                return StatusCode(500, "An error occurred while deleting a team");
            }
        }
    }
}
