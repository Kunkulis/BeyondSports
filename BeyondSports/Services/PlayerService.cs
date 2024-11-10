using AutoMapper;
using BeyondSports.Data;
using BeyondSports.DTOs;
using BeyondSports.Models;

namespace BeyondSports.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ILogger<PlayerService> _logger;
        private readonly IMapper _mapper;

        public PlayerService(IPlayerRepository playerRepository, ITeamRepository teamRepository, ILogger<PlayerService> logger, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync()
        {
            _logger.LogInformation("Fetching all players.");
            var players = await _playerRepository.GetAllPlayersAsync();
            _logger.LogInformation($"Fetched {players.Count()} players.");
            return _mapper.Map<List<PlayerDto>>(players);
        }

        public async Task<PlayerDto> GetPlayerByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching player with ID {id}.");
            var player = await _playerRepository.GetPlayerByIdAsync(id);
            if (player == null)
            {
                _logger.LogWarning($"Player with ID {id} not found.");
                return null!;
            }
            _logger.LogInformation($"Fetched player with ID {id}.");
            return _mapper.Map<PlayerDto>(player);
        }

        public async Task<(bool Success, string ErrorMessage, PlayerDto Player)> AddPlayerAsync(CreatePlayerDto newPlayer)
        {
            _logger.LogInformation($"Adding new player with name {newPlayer.Name} to team {newPlayer.TeamId}.");
            var team = await _teamRepository.GetTeamByIdAsync(newPlayer.TeamId);
            if (team == null)
            {
                _logger.LogWarning($"Team with ID {newPlayer.TeamId} not found.");
                return (false, $"Team with Id {newPlayer.TeamId} doesn't exist", null!);
            }

            var playerExists = await _playerRepository.PlayerExistsInTeamAsync(newPlayer.Name, newPlayer.TeamId, newPlayer.BirthDate);
            if (playerExists)
            {
                _logger.LogWarning($"A player with name {newPlayer.Name} already exists in team {newPlayer.TeamId}.");
                return (false, "A player with this name already exists in the specified team.", null!);
            }

            var numberExistsInTeam = await _playerRepository.PlayerNumberExistsInTeamAsync(newPlayer.Number, newPlayer.TeamId);
            if (numberExistsInTeam)
            {
                _logger.LogWarning($"The number {newPlayer.Number} is already assigned to another player in team {newPlayer.TeamId}.");
                return (false, $"The number {newPlayer.Number} is already assigned to another player in this team.", null!);
            }

            var player = _mapper.Map<Player>(newPlayer);
            await _playerRepository.AddPlayerAsync(player);

            var playerDto = _mapper.Map<PlayerDto>(player);
            _logger.LogInformation($"Added new player with ID {playerDto.Id}.");
            return (true, null!, playerDto);
        }

        public async Task<(bool Success, string ErrorMessage)> UpdatePlayerAsync(int id, UpdatePlayerDto updatePlayer)
        {
            _logger.LogInformation($"Updating player with ID {id}.");
            var player = await _playerRepository.GetPlayerByIdAsync(id);
            if (player == null)
            {
                _logger.LogWarning($"Player with ID {id} not found.");
                return (false, $"Player with ID {id} not found.");
            }

            if (!string.IsNullOrEmpty(updatePlayer.TeamName))
            {
                var team = await _teamRepository.GetTeamByNameAsync(updatePlayer.TeamName);
                if (team == null)
                {
                    _logger.LogWarning($"Team with name {updatePlayer.TeamName} not found.");
                    return (false, $"Team '{updatePlayer.TeamName}' not found.");
                }

                player.TeamId = team.Id;
            }

            _mapper.Map(updatePlayer, player);
            await _playerRepository.UpdatePlayerAsync(player);
            _logger.LogInformation($"Updated player with ID {id}.");
            return (true, null!);
        }

        public async Task<(bool Success, string ErrorMessage)> DeletePlayerAsync(int id)
        {
            _logger.LogInformation($"Deleting player with ID {id}.");
            var player = await _playerRepository.GetPlayerByIdAsync(id);
            if (player == null)
            {
                _logger.LogWarning($"Player with ID {id} not found.");
                return (false, $"Player with Id {id} doesn't exist");
            }

            await _playerRepository.DeletePlayerAsync(id);
            _logger.LogInformation($"Deleted player with ID {id}.");
            return (true, null!);
        }
    }
}
