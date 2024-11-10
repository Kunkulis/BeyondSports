using AutoMapper;
using BeyondSports.Data;
using BeyondSports.DTOs;
using BeyondSports.Models;

namespace BeyondSports.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _repository;
        private readonly ILogger<TeamService> _logger;
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository repository, ILogger<TeamService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeamDto>> GetAllTeamsAsync()
        {
            _logger.LogInformation("Fetching all teams.");
            var teams = await _repository.GetAllTeamsAsync();
            _logger.LogInformation($"Fetched {teams.Count()} teams.");
            return _mapper.Map<List<TeamDto>>(teams);            
        }
        public async Task<TeamDto> GetTeamByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching team with ID {id}.");
            var team = await _repository.GetTeamByIdAsync(id);
            if (team == null)
            {
                _logger.LogWarning($"Team with ID {id} not found.");
                return null!;
            }
            _logger.LogInformation($"Fetched team with ID {id}.");
            return _mapper.Map<TeamDto>(team);
        }

        public async Task<(bool Success, string ErrorMessage, TeamDto Team)> CreateTeamAsync(CreateTeamDto newTeam)
        {
            _logger.LogInformation($"Creating new team with name {newTeam.Name}.");
            var teamExists = await _repository.GetTeamByNameAsync(newTeam.Name);
            if (teamExists != null)
            {
                _logger.LogWarning($"Team with name {newTeam.Name} already exists.");
                return (false, "A team with this name already exists.", null!);
            }

            var team = _mapper.Map<Team>(newTeam);
            await _repository.CreateTeamAsync(team);
            _logger.LogInformation($"Created new team with name {newTeam.Name}.");

            var teamDto = _mapper.Map<TeamDto>(team);
            return (true, null!, teamDto);
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteTeamAsync(int id)
        {
            _logger.LogInformation($"Deleting team with ID {id}.");
            var team = await _repository.GetTeamByIdAsync(id);
            if (team == null)
            {
                _logger.LogWarning($"Team with ID {id} not found.");
                return (false, "Team not found.");
            }

            await _repository.DeleteTeamAsync(id);
            _logger.LogInformation($"Deleted team with ID {id}.");
            return (true, null!);
        }
    }
}
