using BeyondSports.Models;

namespace BeyondSports.Data
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetAllPlayersAsync();
        Task<Player?> GetPlayerByIdAsync(int id);
        Task AddPlayerAsync(Player player);
        Task UpdatePlayerAsync(Player player);
        Task DeletePlayerAsync(int id);
        Task<bool> PlayerExistsInTeamAsync(string name, int teamId, DateOnly birthDate);
        Task<bool> PlayerNumberExistsInTeamAsync(int number, int teamId);
    }
}