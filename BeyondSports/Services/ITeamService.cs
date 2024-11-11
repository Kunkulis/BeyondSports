using BeyondSports.DTO;

namespace BeyondSports.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamDto>> GetAllTeamsAsync();
        Task<TeamDto> GetTeamByIdAsync(int id);
        Task<(bool Success, string ErrorMessage, TeamDto Team)> CreateTeamAsync(CreateTeamDto newTeam);
        Task<(bool Success, string ErrorMessage)> DeleteTeamAsync(int id);
    }
}
