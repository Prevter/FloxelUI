using System;
using System.Collections.Generic;
using System.Linq;

namespace FloxelLib.Common;

public static class Animations
{
    public delegate double EasingFunction(double x);

    public static double EaseInOutSine(double x)
    {
        return -(Math.Cos(Math.PI * x) - 1) / 2;
    }

    public static double Linear(double x)
    {
        return x;
    }

    public static T MapValues<T>(double progress, EasingFunction func, Dictionary<double, T> values)
    {
        var sortedValues = values.Select(v => new { progress = v.Key, value = v.Value })
            .OrderBy(v => v.progress).ToArray();
        var nextValue = sortedValues.FirstOrDefault(v => v.progress > progress);

        if (nextValue == default)
            return sortedValues.Last().value;

        if (nextValue == sortedValues.First())
            return nextValue.value;

        var previousValue = sortedValues[Array.IndexOf(sortedValues, nextValue) - 1];
        var valueProgress = (progress - previousValue.progress) / (nextValue.progress - previousValue.progress);
        var easedProgress = func(valueProgress);
        var valueDifference = Convert.ToDouble(nextValue.value) - Convert.ToDouble(previousValue.value);
        var value = Convert.ToDouble(previousValue.value) + valueDifference * easedProgress;

        return (T)Convert.ChangeType(value, typeof(T));
    }
}
