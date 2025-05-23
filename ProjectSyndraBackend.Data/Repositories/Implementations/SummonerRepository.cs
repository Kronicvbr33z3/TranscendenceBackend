// SummonerRepository.cs

using Microsoft.EntityFrameworkCore;
using ProjectSyndraBackend.Data.Models.LoL.Account;
using ProjectSyndraBackend.Data.Repositories.Interfaces;

namespace ProjectSyndraBackend.Data.Repositories.Implementations;

public class SummonerRepository(ProjectSyndraContext context, IRankRepository rankRepository) : ISummonerRepository
{
    public async Task<Summoner?> GetSummonerByPuuidAsync(string puuid, CancellationToken cancellationToken)
    {
        return await context.Summoners.FirstOrDefaultAsync(x => x.Puuid == puuid, cancellationToken);
    }

    public async Task AddOrUpdateSummonerAsync(Summoner summoner, CancellationToken cancellationToken)
    {
        var existingSummoner =
            await GetSummonerByPuuidAsync(summoner.Puuid!, cancellationToken);
        if (existingSummoner == null)
        {
            context.Summoners.Add(summoner);
        }
        else
        {
            existingSummoner.Puuid = summoner.Puuid;
            existingSummoner.AccountId = summoner.AccountId;
            existingSummoner.ProfileIconId = summoner.ProfileIconId;
            existingSummoner.RevisionDate = summoner.RevisionDate;
            existingSummoner.SummonerLevel = summoner.SummonerLevel;
            existingSummoner.GameName = summoner.GameName;
            existingSummoner.TagLine = summoner.TagLine;
            existingSummoner.SummonerName = summoner.SummonerName;
            existingSummoner.PlatformRegion = summoner.PlatformRegion;
            existingSummoner.Region = summoner.Region;
            existingSummoner.RiotSummonerId = summoner.RiotSummonerId;

            await rankRepository.AddOrUpdateRank(existingSummoner.Ranks.ToList());


        }

        await context.SaveChangesAsync(cancellationToken);
    }
}