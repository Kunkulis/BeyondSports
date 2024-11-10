using BeyondSports.DTOs;

namespace BeyondSports.Services
{
    public interface IPlayerService
    {
        Task<IEnumerable<PlayerDto>> GetAllPlayersAsync();
        Task<PlayerDto> GetPlayerByIdAsync(int id);
        Task<(bool Success, string ErrorMessage, PlayerDto Player)> AddPlayerAsync(CreatePlayerDto newPlayer);
        Task<(bool Success, string ErrorMessage)> UpdatePlayerAsync(int id, UpdatePlayerDto updatePlayer);
        Task<(bool Success, string ErrorMessage)> DeletePlayerAsync(int id);
    }
}
