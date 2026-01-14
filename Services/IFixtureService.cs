// Services/FixtureService.cs
using FixtureDisplay.Models;

namespace FixtureDisplay.Services
{
    public interface IFixtureService
    {
        Task<List<Team>> GetAllTeamsAsync();
        Task<List<SunderlandFixture>> GetFixturesByTeamsAsync(string homeTeamCode, string awayTeamCode);
    }
}