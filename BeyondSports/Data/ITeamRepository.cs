using BeyondSports.Models;

namespace BeyondSports.Data
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAllTeamsAsync();
        Task<Team?> GetTeamByIdAsync(int id);
        Task<Team?> GetTeamByNameAsync(string teamName);
        Task CreateTeamAsync(Team team);
        Task DeleteTeamAsync(int id);        
    }
}
