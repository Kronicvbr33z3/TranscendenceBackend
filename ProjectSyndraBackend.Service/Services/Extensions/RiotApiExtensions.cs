﻿using Camille.Enums;
using Camille.RiotGames;
using Microsoft.EntityFrameworkCore;
using ProjectSyndraBackend.Data;
using ProjectSyndraBackend.Data.Models.Account;
using ProjectSyndraBackend.Data.Models.Match;

namespace ProjectSyndraBackend.Service.Services.Extensions;

public static class RiotApiExtensions
{
    public static async Task<Summoner> GetSummoner(this RiotGamesApi riotApi, string summonerId,
        PlatformRoute platformRoute, CancellationToken cancellationToken = default)
    {
        var summoner = await riotApi.SummonerV4().GetBySummonerIdAsync(platformRoute, summonerId, cancellationToken);
        
        var current = new Summoner
        {
            SummonerId = summoner.Puuid,
            RiotSummonerId = summoner.Id,
            Puuid = summoner.Puuid,
            AccountId = summoner.AccountId,
            ProfileIconId = summoner.ProfileIconId,
            RevisionDate = summoner.RevisionDate,
            SummonerLevel = summoner.SummonerLevel
        };

        // fetch the account from the puuid, can use any region here.
        var account = await riotApi.AccountV1()
            .GetByPuuidAsync(RegionalRoute.AMERICAS, summoner.Puuid, cancellationToken);

        current.GameName = account.GameName;
        current.TagLine = account.TagLine;
        current.SummonerName = account.GameName + "#" + account.TagLine;

        return current;
    }

    public static async Task<Summoner> GetSummonerByPuuid(this RiotGamesApi riotApi, string puuid,
        PlatformRoute platformRoute, CancellationToken cancellationToken = default)
    {
        var summoner = await riotApi.SummonerV4().GetByPUUIDAsync(platformRoute, puuid, cancellationToken);
        
        var current = new Summoner
        {
            SummonerId = summoner.Puuid,
            RiotSummonerId = summoner.Id,
            Puuid = summoner.Puuid,
            AccountId = summoner.AccountId,
            ProfileIconId = summoner.ProfileIconId,
            RevisionDate = summoner.RevisionDate,
            SummonerLevel = summoner.SummonerLevel
        };

        // fetch the account from the puuid, can use any region here.
        var account = await riotApi.AccountV1()
            .GetByPuuidAsync(RegionalRoute.AMERICAS, summoner.Puuid, cancellationToken);

        current.GameName = account.GameName;
        current.TagLine = account.TagLine;
        current.SummonerName = account.GameName + "#" + account.TagLine;

        return current;
    }

    public static async Task<Match?> GetMatchDetails(this RiotGamesApi riotApi, string matchId,
        RegionalRoute regionalRoute, PlatformRoute platformRoute, ProjectSyndraContext data,
        CancellationToken cancellationToken = default)
    {
        var details = await riotApi.MatchV5().GetMatchAsync(regionalRoute, matchId, cancellationToken);

        if (details == null) return null;

        // make sure the match doesn't already exist in the database.
        if (await data.Matches.AnyAsync(x => x.MatchId == details.Metadata.MatchId, cancellationToken)) return null;

        var match = new Match
        {
            MatchId = details.Metadata.MatchId,
            MatchDate = details.Info.GameCreation,
            Duration = (int)details.Info.GameDuration,
            Patch = details.Info.GameVersion,
            EndOfGameResult = details.Info.EndOfGameResult
        };

        var localSummoners = new List<Summoner>();
        var localMatchDetails = new List<MatchDetail>();

        foreach (var participant in details.Info.Participants)
        {
            // if the summoner is not found in our database, we will create a new one.
            var summoner =
                await data.Summoners.FirstOrDefaultAsync(x => x.SummonerId == participant.Puuid, cancellationToken);

            if (summoner == null)
            {
                summoner = await riotApi.GetSummonerByPuuid(participant.Puuid, platformRoute, cancellationToken);
                data.Summoners.Add(summoner);
            }

            match.MatchSummoners.Add(new MatchSummoner
            {
                MatchId = match.MatchId,
                Match = match,
                SummonerId = summoner.SummonerId,
                Summoner = summoner
            });
            localSummoners.Add(summoner);
        }


        foreach (var info in details.Info.Participants)
        {
            var summoner = localSummoners.FirstOrDefault(x => x.SummonerId == info.Puuid);
            var items = new List<int>();

            var matchDetail = new MatchDetail
            {
                Kills = info.Kills,
                Deaths = info.Deaths,
                Assists = info.Assists,
                MatchId = match.MatchId,
                Win = info.Win,
                SummonerSpell1 = info.Summoner1Id,
                SummonerSpell2 = info.Summoner2Id,
                Lane = info.Lane,
                Role = info.Role,
                ChampionName = info.ChampionName,
                ChampionId = (int)info.ChampionId,
                Match = match,
                SummonerId = summoner!.SummonerId!,
                Summoner = summoner
            };

            items.Add(info.Item0);
            items.Add(info.Item1);
            items.Add(info.Item2);
            items.Add(info.Item3);
            items.Add(info.Item4);
            items.Add(info.Item5);
            items.Add(info.Item6);

            matchDetail.Items = items;


            var localRunes = new Runes();

            foreach (var rune in info.Perks.Styles)
                if (rune.Description.Equals("primaryStyle"))
                {
                    localRunes.PrimaryStyle = rune.Style;

                    localRunes.Perk0 = rune.Selections[0].Perk;
                    localRunes.Perk1 = rune.Selections[1].Perk;
                    localRunes.Perk2 = rune.Selections[2].Perk;
                    localRunes.Perk3 = rune.Selections[3].Perk;

                    // for each selection append all var1, var2, var3 to the rune vars.
                    localRunes.RuneVars0[0] = rune.Selections[0].Var1;
                    localRunes.RuneVars1[0] = rune.Selections[1].Var1;
                    localRunes.RuneVars2[0] = rune.Selections[2].Var1;
                    localRunes.RuneVars3[0] = rune.Selections[3].Var1;
                        
                    localRunes.RuneVars0[1] = rune.Selections[0].Var2;
                    localRunes.RuneVars1[1] = rune.Selections[1].Var2;
                    localRunes.RuneVars2[1] = rune.Selections[2].Var2;
                    localRunes.RuneVars3[1] = rune.Selections[3].Var2;
                        
                    localRunes.RuneVars0[2] = rune.Selections[0].Var3;
                    localRunes.RuneVars1[2] = rune.Selections[1].Var3;
                    localRunes.RuneVars2[2] = rune.Selections[2].Var3;
                    localRunes.RuneVars3[2] = rune.Selections[3].Var3;
                }
                else if (rune.Description.Equals("subStyle"))
                {
                    localRunes.SubStyle = rune.Style;

                    localRunes.Perk4 = rune.Selections[0].Perk;
                    localRunes.Perk5 = rune.Selections[1].Perk;

                    // for each selection append all var1, var2, var3 to the rune vars.
                    localRunes.RuneVars4[0] = rune.Selections[0].Var1;
                    localRunes.RuneVars5[0] = rune.Selections[1].Var1;
                    
                    localRunes.RuneVars4[1] = rune.Selections[0].Var2;
                    localRunes.RuneVars5[1] = rune.Selections[1].Var2;
                    
                    localRunes.RuneVars4[2] = rune.Selections[0].Var3;
                    localRunes.RuneVars5[2] = rune.Selections[1].Var3;
                    
                    
                    
                }

            localRunes.StatDefense = info.Perks.StatPerks.Defense;
            localRunes.StatFlex = info.Perks.StatPerks.Flex;
            localRunes.StatOffense = info.Perks.StatPerks.Offense;

            matchDetail.Runes = localRunes;

            localMatchDetails.Add(matchDetail);
        }

        match.MatchDetails = localMatchDetails;


        await data.Matches.AddAsync(match, cancellationToken);
        await data.SaveChangesAsync(cancellationToken);

        return match;
    }
}