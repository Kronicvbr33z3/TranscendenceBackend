using Camille.Enums;
using Camille.RiotGames;
using ProjectSyndraBackend.Data.Models.LoL.Account;
using ProjectSyndraBackend.Service.Services.RiotApi.Interfaces;

namespace ProjectSyndraBackend.Service.Services.RiotApi.Implementations;

// SummonerService.cs

public class SummonerService(RiotGamesApi riotApi, IRankService rankService)
    : ISummonerService
{
    public async Task<Summoner> GetSummonerByIdAsync(string summonerId, PlatformRoute platformRoute,
        CancellationToken cancellationToken = default)
    {
        var summoner = await riotApi.SummonerV4().GetBySummonerIdAsync(platformRoute, summonerId, cancellationToken);
        return await CreateSummonerAsync(summoner, platformRoute, cancellationToken);
    }

    public async Task<Summoner> GetSummonerByPuuidAsync(string puuid, PlatformRoute platformRoute,
        CancellationToken cancellationToken = default)
    {
        var summoner = await riotApi.SummonerV4().GetByPUUIDAsync(platformRoute, puuid, cancellationToken);
        return await CreateSummonerAsync(summoner, platformRoute, cancellationToken);
    }

    private async Task<Summoner> CreateSummonerAsync(Camille.RiotGames.SummonerV4.Summoner summoner,
        PlatformRoute platformRoute, CancellationToken cancellationToken)
    {
        var current = new Summoner
        {
            RiotSummonerId = summoner.Id,
            Puuid = summoner.Puuid,
            AccountId = summoner.AccountId,
            ProfileIconId = summoner.ProfileIconId,
            RevisionDate = summoner.RevisionDate,
            SummonerLevel = summoner.SummonerLevel,
            PlatformRegion = platformRoute.ToString(),
            Region = platformRoute.ToRegional().ToString()
        };

        var account = await riotApi.AccountV1()
            .GetByPuuidAsync(RegionalRoute.AMERICAS, summoner.Puuid, cancellationToken);
        current.GameName = account.GameName;
        current.TagLine = account.TagLine;
        current.SummonerName = account.GameName + "#" + account.TagLine;


        var latestRank = await rankService.GetRankedDataAsync(current.RiotSummonerId, platformRoute, cancellationToken);

        if (latestRank.Count > 0)
        {
            current.Ranks = latestRank;
        }
        else
        {
            current.Ranks = [];
        }
        return current;
    }
}