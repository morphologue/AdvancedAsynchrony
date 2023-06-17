using System.Threading.Channels;

namespace SingleThreadAsynchrony;

internal sealed class SequentialSynchronizationContext : SynchronizationContext
{
    private readonly Channel<(SendOrPostCallback Callback, object? State)> _channel
        = Channel.CreateUnbounded<(SendOrPostCallback Callback, object? State)>();

    internal SequentialSynchronizationContext() { }

    public override void Send(SendOrPostCallback d, object? state) => _channel.Writer.TryWrite((d, state));

    public override void Post(SendOrPostCallback d, object? state) => _channel.Writer.TryWrite((d, state));

    internal void Run(Func<Task> program)
    {
        var programTask = program();
        while (!programTask.IsCompleted)
        {
            if (_channel.Reader.TryRead(out var work))
            {
                base.Send(work.Callback, work.State);
            }
        }
    }
}
