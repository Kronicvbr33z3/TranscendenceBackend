namespace ProjectSyndraBackend.Data.Models.Service.Helpers;

public class UnitWinPercent
{
    public int Id { get; set; }
    public int NumberOfGames { get; set; }
    public required float WinRate { get; set; }
    public required string Type { get; set; }
    public required object Unit { get; set; }
}