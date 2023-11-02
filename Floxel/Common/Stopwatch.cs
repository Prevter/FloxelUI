using System;

namespace FloxelLib.Common;

public static class Stopwatch
{
    private const long TicksPerMillisecond = 10000;
    private const long TicksPerSecond = TicksPerMillisecond * 1000;
    private static readonly double s_tickFrequency = (double)TicksPerSecond / System.Diagnostics.Stopwatch.Frequency;

    public static long GetTimestamp() => System.Diagnostics.Stopwatch.GetTimestamp();

    public static TimeSpan GetTimePassed(long starttime)
    {
        long endtime = System.Diagnostics.Stopwatch.GetTimestamp();
        return GetTimePassed(starttime, endtime);
    }

    public static TimeSpan GetTimePassed(long starttime, long endtime)
    {
        return new TimeSpan((long)((endtime - starttime) * s_tickFrequency));
    }
}
