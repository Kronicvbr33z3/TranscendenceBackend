// ISummonerRepository.cs

using ProjectSyndraBackend.Data.Models.LoL.Account;

namespace ProjectSyndraBackend.Data.Repositories;

public interface ISummonerRepository
{
    Task<Summoner?> GetSummonerByPuuidAsync(string puuid, CancellationToken cancellationToken);
    Task AddOrUpdateSummonerAsync(Summoner summoner, CancellationToken cancellationToken);
}