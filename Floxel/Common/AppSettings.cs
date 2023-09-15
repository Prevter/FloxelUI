using System;
using System.IO;
using System.Text.Json;

namespace FloxelLib.Common;

public static class AppSettings
{
	public static void Save<T>(T settings, bool localPath = false)
	{
		var path = GetPath<T>(localPath);
		var json = JsonSerializer.Serialize(settings);
		File.WriteAllText(path, json);
	}

	public static T Load<T>(bool localPath = false) where T : new()
	{
		if (!File.Exists(GetPath<T>()))
			return new T();

		var json = File.ReadAllText(GetPath<T>(localPath));
		return JsonSerializer.Deserialize<T>(json) ?? new T();
	}

	public static string GetPath<T>(bool localPath = false)
	{
		if (localPath)
			return typeof(T).Name + ".json";

		var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Floxel.GetApplicationName());
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		var fileName = typeof(T).Name + ".json";
		return Path.Combine(path, fileName);
	}
}
