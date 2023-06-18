namespace Morphologue.Examples.AdvancedAsynchrony.Logging;

internal class ThreadIdConsoleLogger : ISimpleLogger
{
    public void Log(string message) => Console.WriteLine($"{Environment.CurrentManagedThreadId}: {message}");
}
