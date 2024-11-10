using BeyondSports.Models;
using Microsoft.EntityFrameworkCore;

namespace BeyondSports.Data
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamRepository> _logger;

        public TeamRepository(ApplicationDbContext context, ILogger<TeamRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            try
            {
                return await _context.Teams.Include(t => t.Players).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all teams.");
                throw;
            }
        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            try
            {
                return await _context.Teams.Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching team with ID {id}.");
                throw;
            }
        }

        public async Task<Team?> GetTeamByNameAsync(string teamName)
        {
            try
            {
                return await _context.Teams.FirstOrDefaultAsync(t => t.Name == teamName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching team with name {teamName}.");
                throw;
            }
        }

        public async Task CreateTeamAsync(Team team)
        {
            try
            {
                _context.Teams.Add(team);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new team.");
                throw;
            }
        }

        public async Task DeleteTeamAsync(int id)
        {
            try
            {
                var team = await _context.Teams.FindAsync(id);
                if (team != null)
                {
                    _context.Teams.Remove(team);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting team with ID {id}.");
                throw;
            }
        }
    }
}
