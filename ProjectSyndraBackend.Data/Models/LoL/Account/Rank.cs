namespace ProjectSyndraBackend.Data.Models.LoL.Account;

public class Rank
{
    public Guid Id { get; set; }
    public string Tier { get; set; } = "";
    public string RankNumber { get; set; } = "";
    public int LeaguePoints { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public string QueueType { get; set; } = "";
    public Guid SummonerId { get; set; }
    public required Summoner Summoner { get; set; }
}