using ProjectSyndraBackend.Service.Services.Analysis;
using ProjectSyndraBackend.Service.Services.Analysis.Interfaces;

namespace ProjectSyndraBackend.Service.Services.Jobs;

// ReSharper disable once UnusedType.Global
public class AnalyzeData(IChampionLoadoutAnalysisService championLoadoutAnalysis) : IJobTask
{
    public async Task Execute(CancellationToken stoppingToken)
    {
        var loadouts = await championLoadoutAnalysis.GetChampionLoadoutsAsync(stoppingToken);
        // do something with the loadouts
        // for now print all the loadouts
        foreach (var loadout in loadouts)
        {
            Console.WriteLine(loadout);
        }
    }
}