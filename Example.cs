using Morphologue.Examples.AdvancedAsynchrony.Logging;
using Morphologue.Examples.AdvancedAsynchrony.Synchronization;

namespace Morphologue.Examples.AdvancedAsynchrony;

internal class Example
{
    private readonly ISimpleLogger _logger;
    private readonly Options _options;
    private readonly RunnableSynchronizationContext _synchronizationContext;

    public Example(
        ISimpleLogger logger,
        Options options,
        RunnableSynchronizationContext synchronizationContext)
    {
        _logger = logger;
        _options = options;
        _synchronizationContext = synchronizationContext;
    }

    internal void Run()
    {
        SynchronizationContext.SetSynchronizationContext(_synchronizationContext);
        _logger.Log("Set SynchronizationContext");

        _synchronizationContext.Run(DriveAsync);
        _logger.Log("Finished run");
    }

    private async Task FlowAsync(int flowNumber)
    {
        _logger.Log($"Starting flow {flowNumber}");
        await Task.Delay(100);
        _logger.Log($"Finished flow {flowNumber}");
    }

    private async Task DriveAsync()
    {
        _logger.Log("Starting example");
        await Task.WhenAll(Enumerable.Range(1, _options.NumTasks).Select(FlowAsync));
        _logger.Log("Finished example");
    }
}
