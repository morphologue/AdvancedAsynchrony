using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Morphologue.Examples.AdvancedAsynchrony.Logging;
using Morphologue.Examples.AdvancedAsynchrony.Synchronization;

namespace Morphologue.Examples.AdvancedAsynchrony;

[DynamicallyAccessedMembers(
    DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)]
[VersionOption("AdvancedAsynchrony 1.0.0.")]
[HelpOption]
// ReSharper disable once ClassNeverInstantiated.Global
internal class Options
{
    [Option("--tasks", Description = "Total number of tasks which will be run (default 5)")]
    [Range(1, int.MaxValue)]
    public int NumTasks { get; set; } = 5;

    [Option("--threads", Description = "Degree of parallelism (default 1, i.e. main thread only)")]
    [Range(1, int.MaxValue)]
    public int NumThreads { get; set; } = 1;

    public void OnExecute()
    {
        var services = new ServiceCollection();
        services
            .AddSingleton(_ => this)
            .AddSingleton<ISimpleLogger, ThreadIdConsoleLogger>()
            .AddSingleton<RunnableSynchronizationContext, ThreadLimitingSynchronizationContext>()
            .AddSingleton<Example>();

        services.BuildServiceProvider().GetRequiredService<Example>().Run();
    }
}
