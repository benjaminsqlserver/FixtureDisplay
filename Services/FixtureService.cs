using System.Data;
using Microsoft.Data.SqlClient;
using FixtureDisplay.Models;

namespace FixtureDisplay.Services
{
    public class FixtureService : IFixtureService
    {
        private readonly string _connectionString;

        public FixtureService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            var teams = new List<Team>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "SELECT TeamId, TeamCode, TeamName FROM Teams ORDER BY TeamName",
                    connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        teams.Add(new Team
                        {
                            TeamId = reader.GetInt32(0),
                            TeamCode = reader.GetString(1),
                            TeamName = reader.GetString(2)
                        });
                    }
                }
            }

            return teams;
        }

        public async Task<List<int>> GetDistinctSeasonsAsync()
        {
            var seasons = new List<int>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "SELECT DISTINCT OriginalSeason FROM HistoricalResults ORDER BY OriginalSeason DESC",
                    connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        seasons.Add(reader.GetInt32(0));
                    }
                }
            }

            return seasons;
        }

        public async Task<List<HistoricalResult>> GetHistoricalResultsByTeamsAndSeasonAsync(
            string homeTeamCode,
            string awayTeamCode,
            int season)
        {
            var results = new List<HistoricalResult>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"SELECT hr.HistoricalResultId, hr.HomeTeamCode, hr.AwayTeamCode, 
                           hr.HomeScore, hr.AwayScore, hr.OriginalSeason, hr.OriginalMatchday,
                           ht.TeamName as HomeTeamName, at.TeamName as AwayTeamName
                           FROM HistoricalResults hr
                           LEFT JOIN Teams ht ON hr.HomeTeamCode = ht.TeamCode
                           LEFT JOIN Teams at ON hr.AwayTeamCode = at.TeamCode
                           WHERE hr.HomeTeamCode = @HomeTeamCode 
                           AND hr.AwayTeamCode = @AwayTeamCode
                           AND hr.OriginalSeason = @Season
                           ORDER BY hr.OriginalMatchday";

                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@HomeTeamCode", homeTeamCode);
                command.Parameters.AddWithValue("@AwayTeamCode", awayTeamCode);
                command.Parameters.AddWithValue("@Season", season);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(new HistoricalResult
                        {
                            HistoricalResultId = reader.GetInt32(0),
                            HomeTeamCode = reader.GetString(1),
                            AwayTeamCode = reader.GetString(2),
                            HomeScore = reader.GetInt32(3),
                            AwayScore = reader.GetInt32(4),
                            OriginalSeason = reader.GetInt32(5),
                            OriginalMatchday = reader.GetInt32(6),
                            HomeTeamName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            AwayTeamName = reader.IsDBNull(8) ? string.Empty : reader.GetString(8)
                        });
                    }
                }
            }

            return results;
        }
    }
}