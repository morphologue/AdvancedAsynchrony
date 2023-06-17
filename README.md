# Non-concurrent asynchrony in .NET

This project is an example of executing multiple `Task`s via [Task.WhenAll()](https://learn.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task.WhenAll?view=net-7.0),
starting and resuming them all on one thread. The effect is asynchrony without concurrency, similar to typical
programming patterns in JavaScript.

Example output:

```
1: Set SynchronizationContext
1: Starting MainAsync
1: Starting flow 1
1: Starting flow 2
1: Starting flow 3
1: Starting flow 4
1: Starting flow 5
1: Finished flow 4
1: Finished flow 5
1: Finished flow 3
1: Finished flow 2
1: Finished flow 1
1: Finished MainAsync
1: Finished everything
```

The `1` represents the thread ID.
