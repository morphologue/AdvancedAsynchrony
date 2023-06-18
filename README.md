# Advanced asynchrony in .NET

This project is an example of how to control the allocation of asynchronous work to threads in .NET.

It is unfortunately impossible to throttle top-level `Task`s as the Task hierarchy is hidden behind internal fields.
This situation is discussed on StackOverflow: https://stackoverflow.com/a/57702536.

Output from `dotnet run -- --help`:

```
AdvancedAsynchrony 1.0.0.

Usage: AdvancedAsynchrony [options]

Options:
  --version     Show version information.
  -?|-h|--help  Show help information.
  --tasks       Total number of tasks which will be run (default 5)
                Default value is: 5.
  --threads     Degree of parallelism (default 1, i.e. main thread only)
                Default value is: 1.
```

Output from `--tasks 15 --thread 3`:

```
1: Set SynchronizationContext
1: Starting example
1: Starting flow 1
1: Starting flow 2
1: Starting flow 3
1: Starting flow 4
1: Starting flow 5
1: Starting flow 6
1: Starting flow 7
1: Starting flow 8
1: Starting flow 9
1: Starting flow 10
1: Starting flow 11
1: Starting flow 12
1: Starting flow 13
1: Starting flow 14
1: Starting flow 15
1: Finished flow 6
13: Finished flow 12
1: Finished flow 5
12: Finished flow 10
1: Finished flow 3
13: Finished flow 4
12: Finished flow 2
13: Finished flow 15
1: Finished flow 1
13: Finished flow 13
1: Finished flow 11
13: Finished flow 9
12: Finished flow 14
13: Finished flow 7
1: Finished flow 8
1: Finished example
1: Finished run
```

The number at the left of each output line is the managed thread ID.
