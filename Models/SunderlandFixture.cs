namespace FixtureDisplay.Models
{
    public class SunderlandFixture
    {
        public int ID { get; set; }
        public string HomeTeamCode { get; set; } = string.Empty;
        public string AwayTeamCode { get; set; } = string.Empty;
        public int? MaximumAwayScore { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
    }
}