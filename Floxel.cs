using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace FloxelUI;

public static class Floxel
{
	[DllImport("dwmapi.dll")]
	private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

	private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
	private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

	public const string LightTheme = "/FloxelUI;component/Themes/LightTheme.xaml";
	public const string DarkTheme = "/FloxelUI;component/Themes/DarkTheme.xaml";

	private static ResourceDictionary? Theme;

	public static string CurrentTheme {
		get => Theme?.Source?.ToString() ?? "";
	}

	public static bool IsDarkMode
	{
		get => CurrentTheme == DarkTheme;
	}

	public static bool IsLightMode
	{
		get => CurrentTheme == LightTheme;
	}

	public static void SetTheme(string path, UriKind uriKind = UriKind.Relative)
	{
		if (string.IsNullOrEmpty(path)) return;

		if (Theme is null)
		{
			Theme = new ResourceDictionary
			{
				Source = new Uri(path, UriKind.Relative)
			};
		}
		else
		{
			Theme.Source = new Uri(path, uriKind);
			Application.Current.Resources.MergedDictionaries.Remove(Theme);
		}

		Application.Current.Resources.MergedDictionaries.Add(Theme);

		UpdateWindows();
	}

	public static void InitApplication()
	{
		string defaultTheme = IsLightTheme() ? "/FloxelUI;component/Themes/LightTheme.xaml" : "/FloxelUI;component/Themes/DarkTheme.xaml";
		SetTheme(defaultTheme);

		Resources dict = new();
		Application.Current.Resources.MergedDictionaries.Add(dict);
	}
	
	private static void UpdateWindows()
	{
		var themeMode = Application.Current.Resources["DarkMode"] as bool?;
		if (themeMode is null) return;

		var windows = Application.Current.Windows;
		for (int i = 0; i < windows.Count; i++)
		{
			var window = windows[i];
			var source = (HwndSource)PresentationSource.FromVisual(window);
			if (source is null) continue;
			UseImmersiveDarkMode(source.Handle, themeMode.Value);
		}
	}

	public static void InitWindow(Window window)
	{
		window.Style = Application.Current.Resources["WindowStyle"] as Style;

		window.Loaded += (object sender, RoutedEventArgs e) =>
		{
			var source = (HwndSource)PresentationSource.FromVisual(window);
			var themeMode = Application.Current.Resources["DarkMode"] as bool?;
			if (themeMode is null) return;
			UseImmersiveDarkMode(source.Handle, themeMode.Value);
		};
	}

	private static bool IsLightTheme()
	{
		using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
		var value = key?.GetValue("AppsUseLightTheme");
		return value is int i && i > 0;
	}

	private static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
	{
		if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17763))
		{
			var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
			if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 18985))
			{
				attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
			}

			int useImmersiveDarkMode = enabled ? 1 : 0;
			return DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
		}

		return false;
	}

	public static string GetApplicationName()
	{
		return System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name ?? "";
	}
}