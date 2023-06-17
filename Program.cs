using SingleThreadAsynchrony;

void LogThread(string message) => Console.WriteLine($"{Environment.CurrentManagedThreadId}: {message}");

async Task DoAsync(int flowNumber)
{
    LogThread($"Starting flow {flowNumber}");
    await Task.Delay(100);
    LogThread($"Finished flow {flowNumber}");
}

async Task MainAsync()
{
    LogThread("Starting MainAsync");
    await Task.WhenAll(Enumerable.Range(1, 5).Select(DoAsync));
    LogThread("Finished MainAsync");
}

var synchronizationContext = new SequentialSynchronizationContext();
SynchronizationContext.SetSynchronizationContext(synchronizationContext);
LogThread("Set SynchronizationContext");

synchronizationContext.Run(MainAsync); 
LogThread("Finished everything");
