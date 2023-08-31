using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace FloxelUI;

public static class StringExtensions
{
	public static bool Parse(this string str, out int result)
	{
		return int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
	}

	public static bool Parse(this string str, out float result)
	{
		return float.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
	}

	public static bool Parse(this string str, out double result)
	{
		return double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
	}
}

public static class ObservableCollectionExtensions
{
	public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
	{
		foreach (var item in items)
			collection.Add(item);
	}

	public static void RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> predicate)
	{
		foreach (var item in collection.Where(predicate).ToList())
			collection.Remove(item);
	}

	public static void ForEach<T>(this ObservableCollection<T> collection, Action<T> action)
	{
		foreach (var item in collection)
			action(item);
	}
}

public static class RandomArrayFillExtensions
{
	public static readonly Random Random = new();

	public static void FillRandom(this int[] array, int min = 0, int max = 100)
	{
		for (int i = 0; i < array.Length; i++)
			array[i] = Random.Next(min, max);
	}

	public static void FillRandom(this float[] array, float min = 0, float max = 100)
	{
		for (int i = 0; i < array.Length; i++)
			array[i] = (float)Random.NextDouble() * (max - min) + min;
	}

	public static void FillRandom(this double[] array, double min = 0, double max = 100)
	{
		for (int i = 0; i < array.Length; i++)
			array[i] = Random.NextDouble() * (max - min) + min;
	}
}