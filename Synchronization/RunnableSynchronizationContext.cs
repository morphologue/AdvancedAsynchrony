namespace Morphologue.Examples.AdvancedAsynchrony.Synchronization;

internal abstract class RunnableSynchronizationContext : SynchronizationContext
{
    internal abstract void Run(Func<Task> program);
}
