// HistoricalRank.cs

namespace ProjectSyndraBackend.Data.Models.LoL.Account;

public class HistoricalRank
{
    public Guid Id { get; set; }
    public string? QueueType { get; set; }
    public string? Tier { get; set; }
    public string? RankNumber { get; set; }
    public int LeaguePoints { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public DateTime DateRecorded { get; set; }
    public required Summoner Summoner { get; set; }
}