using BeyondSports.Models;
using Microsoft.EntityFrameworkCore;

namespace BeyondSports.Data
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlayerRepository> _logger;

        public PlayerRepository(ApplicationDbContext context, ILogger<PlayerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            try
            {
                return await _context.Players.Include(p => p.Team).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all players.");
                throw;
            }
        }

        public async Task<Player?> GetPlayerByIdAsync(int id)
        {
            try
            {
                return await _context.Players.Include(p => p.Team).FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching player with ID {id}.");
                throw;
            }
        }

        public async Task AddPlayerAsync(Player player)
        {
            try
            {
                var maxId = await _context.Players.MaxAsync(p => (int?)p.Id) ?? 0;
                player.Id = maxId + 1;
                await _context.Players.AddAsync(player);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new player.");
                throw;
            }
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            try
            {
                _context.Entry(player).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating player with ID {player.Id}.");
                throw;
            }
        }

        public async Task DeletePlayerAsync(int id)
        {
            try
            {
                var player = await _context.Players.FindAsync(id);
                if (player != null)
                {
                    _context.Players.Remove(player);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting player with ID {id}.");
                throw;
            }
        }

        public async Task<bool> PlayerExistsInTeamAsync(string name, int teamId, DateOnly birthDate)
        {
            try
            {
                return await _context.Players
                    .AnyAsync(p => p.Name == name && p.TeamId == teamId && p.BirthDate == birthDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if player exists in team.");
                throw;
            }
        }

        public async Task<bool> PlayerNumberExistsInTeamAsync(int number, int teamId)
        {
            try
            {
                return await _context.Players
                    .AnyAsync(p => p.Number == number && p.TeamId == teamId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if player number exists in team.");
                throw;
            }
        }
    }
}
