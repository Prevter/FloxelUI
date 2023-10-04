using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace FloxelLib;

public static class Floxel
{
	[DllImport("dwmapi.dll")]
	private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

	private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
	private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

	public const string LightTheme = "/Floxel;component/Themes/LightTheme.xaml";
	public const string DarkTheme = "/Floxel;component/Themes/DarkTheme.xaml";

	private static ResourceDictionary? Theme;

	public static string CurrentTheme => Theme?.Source?.ToString() ?? "";

	public static bool IsDarkMode => CurrentTheme == DarkTheme;

	public static bool IsLightMode => CurrentTheme == LightTheme;

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

	public static void LoadTheme()
	{
		var defaultTheme = IsSystemLightTheme() ? LightTheme : DarkTheme;
		SetTheme(defaultTheme);
	}

	public static void InitApplication()
	{
		/* Resources dict = new();
		Application.Current.Resources.MergedDictionaries.Add(dict);*/
	}

	private static void UpdateWindows()
	{
		if (Application.Current.Resources["DarkMode"] is not bool themeMode) return;

		var windows = Application.Current.Windows;
		for (var i = 0; i < windows.Count; i++)
		{
			var window = windows[i];
			if (window == null) continue;
			if (PresentationSource.FromVisual(window) is not HwndSource source) continue;
			UseImmersiveDarkMode(source.Handle, themeMode);
		}
	}

	public static void InitWindow(Window window)
	{
		window.Style = Application.Current.Resources["WindowStyle"] as Style;

		window.Loaded += (sender, e) =>
		{
			var source = PresentationSource.FromVisual(window) as HwndSource;
			if (Application.Current.Resources["DarkMode"] is not bool themeMode) return;
			if (source is not null) UseImmersiveDarkMode(source.Handle, themeMode);
		};
	}

	public static bool IsSystemLightTheme()
	{
		using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
		var value = key?.GetValue("AppsUseLightTheme");
		return value is int and > 0;
	}

	private static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
	{
		if (!OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17763)) return false;
		var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
		if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 18985))
		{
			attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
		}

		var useImmersiveDarkMode = enabled ? 1 : 0;
		return DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
	}

	public static string GetApplicationName()
	{
		return System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name ?? "";
	}
}