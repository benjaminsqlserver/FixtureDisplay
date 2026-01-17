namespace FixtureDisplay.Models
{
    public class HistoricalResult
    {
        public int HistoricalResultId { get; set; }
        public string HomeTeamCode { get; set; } = string.Empty;
        public string AwayTeamCode { get; set; } = string.Empty;
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public int OriginalSeason { get; set; }
        public int OriginalMatchday { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
    }
}