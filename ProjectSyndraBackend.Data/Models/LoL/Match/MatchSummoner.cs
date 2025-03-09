using ProjectSyndraBackend.Data.Models.LoL.Account;

namespace ProjectSyndraBackend.Data.Models.LoL.Match;

public class MatchSummoner
{
    public string? MatchId { get; set; }
    public Match? Match { get; set; }

    public string? SummonerId { get; set; }
    public Summoner? Summoner { get; set; }
}