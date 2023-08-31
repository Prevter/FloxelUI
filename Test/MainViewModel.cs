using FloxelUI;
using FloxelUI.MVVM;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Test;

public class MainViewModel : BaseViewModel
{
	private RelayCommand? _toggleDarkModeCommand;
	private string _darkModeToggleIcon = Floxel.IsDarkMode ? "WhiteBalanceSunny" : "MoonWaningCrescent";

	public string DarkModeToggleIcon
	{
		get => _darkModeToggleIcon;
		set => SetField(ref _darkModeToggleIcon, value);
	}

	public RelayCommand ToggleDarkModeCommand
	{
		get => _toggleDarkModeCommand ??= new RelayCommand((e) =>
		{
			if (Floxel.IsDarkMode)
			{
				DarkModeToggleIcon = "MoonWaningCrescent";
				Floxel.SetTheme(Floxel.LightTheme);
			}
			else if (Floxel.IsLightMode)
			{
				DarkModeToggleIcon = "WhiteBalanceSunny";
				Floxel.SetTheme(Floxel.DarkTheme);
			}
		});
	}

	private string _buttonVariant = "Contained", _buttonColor = "Primary";
	private bool _buttonEnabled = true, _buttonIcon = false;
	private RelayCommand? _setButtonVariant;
	private RelayCommand? _setButtonColor;
	private Style _playgroundButtonStyle;

	public bool ButtonEnabled
	{
		get => _buttonEnabled;
		set 
		{
			SetField(ref _buttonEnabled, value);
			ReloadButton();
		}
	}

	public bool ButtonIcon
	{
		get => _buttonIcon;
		set
		{
			SetField(ref _buttonIcon, value);
			ReloadButton();
		}
	}

	public RelayCommand SetButtonVariant
	{
		get => _setButtonVariant ??= new RelayCommand((variantObj) =>
		{
			var variant = (string)variantObj;
			_buttonVariant = variant;
			ReloadButton();
		});
	}

	public RelayCommand SetButtonColor
	{
		get => _setButtonColor ??= new RelayCommand((colorObj) =>
		{
			var color = (string)colorObj;
			_buttonColor = color;
			ReloadButton();
		});
	}

	public Style PlaygroundButtonStyle
	{
		get => _playgroundButtonStyle;
		set => SetField(ref _playgroundButtonStyle, value);
	}

	public void ReloadButton()
	{
		string key = $"{_buttonColor}{_buttonVariant}Button";
		PlaygroundButtonStyle = (Style)Application.Current.Resources[key];
	}

	public MainViewModel()
	{
		ReloadButton();
	}
}