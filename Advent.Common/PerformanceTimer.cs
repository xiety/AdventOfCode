namespace System.Diagnostics;

public sealed class PerformanceTimer(Action<TimeSpan> action) : IDisposable
{
    readonly long startTime = Stopwatch.GetTimestamp();

    public TimeSpan Elapsed
        => Stopwatch.GetElapsedTime(startTime, Stopwatch.GetTimestamp());

    public void Dispose()
        => action(Elapsed);

    public static PerformanceTimer CreateConsole()
        => new(a => Console.WriteLine($"Timer: {a}"));
}
