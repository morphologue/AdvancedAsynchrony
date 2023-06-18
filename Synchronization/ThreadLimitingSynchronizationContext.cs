using System.Threading.Tasks.Dataflow;

namespace Morphologue.Examples.AdvancedAsynchrony.Synchronization;

internal class ThreadLimitingSynchronizationContext : RunnableSynchronizationContext
{
    private readonly record struct Work(SendOrPostCallback Callback, object? State);

    private readonly BufferBlock<Work> _buffer = new();
    private readonly int _numThreads;

    public ThreadLimitingSynchronizationContext(Options options) => _numThreads = options.NumThreads;

    #region SynchronizationContext

    public override void Send(SendOrPostCallback d, object? state) => WriteWork(d, state);

    public override void Post(SendOrPostCallback d, object? state) => WriteWork(d, state);

    private void WriteWork(SendOrPostCallback d, object? state)
    {
        if (!_buffer.Post(new Work(d, state)))
        {
            throw new InvalidOperationException("Buffer unavailable");
        }
    }

    #endregion

    #region Run

    internal override void Run(Func<Task> program)
    {
        using var cts = new CancellationTokenSource();
        var programTask = program();
        
        var threads = Enumerable
            .Range(0, _numThreads - 1)
            // The Thread.Join()s below prevent usage of disposed CancellationTokenSource.
            // ReSharper disable once AccessToDisposedClosure
            .Select(_ => new Thread(() => ReceiveContinually(programTask, cts)))
            .ToList();
        
        foreach (var thread in threads)
        {
            thread.Start();
        }
        
        // Use and block the main thread, too.
        ReceiveContinually(programTask, cts);

        // Prevent usage of CancellationTokenSource after disposal.
        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    private void ReceiveContinually(IAsyncResult programTask, CancellationTokenSource cts)
    {
        var ct = cts.Token;
        try
        {
            while (!programTask.IsCompleted)
            {
                var work = _buffer.Receive(ct);
                base.Send(work.Callback, work.State);
            }
            cts.Cancel();
        }
        catch (Exception) when (ct.IsCancellationRequested)
        { }
    }

    #endregion
}
