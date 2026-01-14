// Services/FixtureService.cs
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
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found");
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            var teams = new List<Team>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT TeamId, TeamCode, TeamName FROM Teams ORDER BY TeamName", connection);

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

        public async Task<List<SunderlandFixture>> GetFixturesByTeamsAsync(string homeTeamCode, string awayTeamCode)
        {
            var fixtures = new List<SunderlandFixture>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"SELECT sf.ID, sf.HomeTeamCode, sf.AwayTeamCode, sf.MaximumAwayScore, 
                           ht.TeamName as HomeTeamName, at.TeamName as AwayTeamName
                           FROM SunderlandFixtures sf
                           LEFT JOIN Teams ht ON sf.HomeTeamCode = ht.TeamCode
                           LEFT JOIN Teams at ON sf.AwayTeamCode = at.TeamCode
                           WHERE sf.HomeTeamCode = @HomeTeamCode AND sf.AwayTeamCode = @AwayTeamCode";

                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@HomeTeamCode", homeTeamCode);
                command.Parameters.AddWithValue("@AwayTeamCode", awayTeamCode);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        fixtures.Add(new SunderlandFixture
                        {
                            ID = reader.GetInt32(0),
                            HomeTeamCode = reader.GetString(1),
                            AwayTeamCode = reader.GetString(2),
                            MaximumAwayScore = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                            HomeTeamName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            AwayTeamName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                        });
                    }
                }
            }

            return fixtures;
        }
    }
}