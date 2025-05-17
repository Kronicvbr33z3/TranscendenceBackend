// ISummonerRepository.cs

using ProjectSyndraBackend.Data.Models.LoL.Account;

namespace ProjectSyndraBackend.Data.Repositories.Interfaces;

public interface ISummonerRepository
{
    Task<Summoner?> GetSummonerByPuuidAsync(string puuid,
        Func<IQueryable<Summoner>, IQueryable<Summoner>>? includes = null,
        CancellationToken cancellationToken = default);

    Task AddOrUpdateSummonerAsync(Summoner summoner, CancellationToken cancellationToken);
}