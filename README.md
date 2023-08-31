## FloxelUI
Material Design UI for Windows Presentation Foundation (WPF). Made for private use, but anyone can use it if they want to.

## What is FloxelUI?
FloxelUI is a UI library for WPF that is based on Google's Material Design guidelines. It is designed to be easy to use and to look good. It's not a one-to-one copy of the Material Design guidelines, but it's main purpose is to look better than the default WPF UI.  
It is also designed to be customizable, so you can change the colors and other things to your liking.  
Library also comes with many utilities that make it easier to use WPF, such as a `RelayCommand` class that can be used to bind commands to buttons, and a `BaseViewModel` class that can be used to bind data to the UI. You can check full list of utilities in the [Features](#features) section.

## How do I use it?
First, you'll need to reference the library in your project. You can do this by adding a reference to the DLL file, or by adding the project to your solution and referencing it that way.  
Then, you'll need to initialize the library. You can do this by calling `Floxel.InitApplication();` in your `App.xaml.cs` file.  
```csharp
public partial class App : Application
{
	public App()
	{
		Floxel.InitApplication();
	}
}
```
Next, you'll need to also initialize every window that you want to use the library in. You can do this by calling `Floxel.InitWindow(this);` in the constructor of the window.
```csharp
public partial class MainWindow : Window
{
	public MainWindow()
	{
		Floxel.InitWindow(this);
		InitializeComponent();
	}
}
```
Doing all of this will make sure all styles, controls and other things are loaded correctly.

## Customization
You can easily change app theme by calling `Floxel.SetTheme(theme);` from anywhere in your code.  
```csharp
Floxel.SetTheme(Floxel.DarkTheme);
```
There are two themes available: `Floxel.DarkTheme` and `Floxel.LightTheme`.  
You can also get the current theme from `Floxel.CurrentTheme`.  
Also, you can pass in any path to a XAML file as a theme.  
```csharp
Floxel.SetTheme("Themes/MyTheme.xaml");
```
or to a local file on your computer
```csharp
Floxel.SetTheme("C:/Users/MyUser/MyTheme.xaml", UrlKind.Absolute);
```


## MVVM utilities
### BaseViewModel
`BaseViewModel` is a class that can be used to bind data to the UI. It implements the `INotifyPropertyChanged` interface, so you can use it to notify the UI when a property changes.  
Here's an example of how to use it:
```csharp
public class MyViewModel : BaseViewModel
{
    private string _myText = "Hello World!";
    public string MyText
    {
        get => _myText;
        set => SetField(ref _myText, value);
    }
}
```
```xml
<TextBox Text="{Binding MyText}"/>
```
Note that you can use `SetField` method to set the value of the field and notify the UI at the same time.

### RelayCommand
In order to bind commands to buttons, you can use the `RelayCommand` class. It is a class that implements the `ICommand` interface, so you can use it to bind commands to buttons.  
Here's an example of how to use it:
```csharp
public class MyViewModel : BaseViewModel
{
    private RelayCommand _myCommand;
    public RelayCommand MyCommand => _myCommand ??= new RelayCommand((arg) => {
        // arg is the CommandParameter you set in XAML
        MessageBox.Show(arg.ToString());
    });
}
```
```xml
<Button Content="Click me!" Command="{Binding MyCommand}" CommandParameter="Hello World!"/>
```



## Features
- [x] Material Design
- [x] Customizable colors
- [x] MVVM utilities (BaseViewModel, RelayCommand)
- [x] Theme support (Dark, Light)
- [x] Application State manager (utility for saving and loading application state)
- [x] Custom MessageBox (with support for themes)
- [x] Material Design Icons ([full icon list](https://pictogrammers.com/library/mdi/))
- [x] Converters for binding values
- [x] Useful extensions for build-in types
- [x] Custom Stopwatch class that is built on top of the System.Diagnostics.Stopwatch class, but uses .NET 7 style of time measurement (doesn't allocate memory on heap)

## Work in progress
### Control Styles
- [x] Window
- [x] RadioButton
- [x] TabControl
- [x] Button 
- [ ] TextBox
- [ ] CheckBox
- [ ] ComboBox
- [ ] Slider
- [ ] ProgressBar
- [ ] ScrollBar
- more coming soon...

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE.md) file for details.

## Credits
- [Material Design Icons](https://pictogrammers.com/library/mdi/)
- [Material Design Color Palette](https://www.materialpalette.com/)