using FixtureDisplay.Models;

namespace FixtureDisplay.Services
{
    public interface IFixtureService
    {
        Task<List<Team>> GetAllTeamsAsync();
        Task<List<int>> GetDistinctSeasonsAsync();
        Task<List<HistoricalResult>> GetHistoricalResultsByTeamsAndSeasonAsync(
            string homeTeamCode,
            string awayTeamCode,
            int season);
    }
}