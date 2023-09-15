using System.Windows;
using FloxelLib;
using FloxelLib.MVVM;

namespace FloxelPlayground.ViewModels;

public sealed partial class ButtonViewModel : BaseViewModel
{
    private string _buttonVariant = "Contained", _buttonColor = "Primary", _buttonCode = "<Button/>";
    private bool _buttonEnabled = true, _buttonIcon = true;
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

    public Style PlaygroundButtonStyle
    {
        get => _playgroundButtonStyle;
        set => SetField(ref _playgroundButtonStyle, value);
    }
    
    public string ButtonCode
    {
        get => _buttonCode;
        set => SetField(ref _buttonCode, value);
    }

    [RelayCommand]
    private void SetButtonVariant(object obj)
    {
        var variant = (string)obj;
        _buttonVariant = variant;
        ReloadButton();
    }

    [RelayCommand]
    private void SetButtonColor(object obj)
    {
        var color = (string)obj;
        _buttonColor = color;
        ReloadButton();
    }
    
    private void ReloadButton()
    {
        var key = $"{_buttonColor}{_buttonVariant}Button";
        PlaygroundButtonStyle = (Style)Application.Current.Resources[key];

        ButtonCode = $"<Button";

        if (key != "PrimaryContainedButton") ButtonCode += $" Style=\"{{DynamicResource {key}}}\"";
        if (!ButtonEnabled) ButtonCode += " IsEnabled=\"False\"";
        
        if (ButtonIcon)
        {
            ButtonCode += @$">
    <Button.Content>
        <StackPanel Orientation=""Horizontal"">
            <fuic:Icon Kind=""Star"" Width=""18"" Margin=""0,0,4,0""/>
            <TextBlock Text=""BUTTON""/>
        </StackPanel>
    </Button.Content>
</Button>";
        }
        else
        {
            ButtonCode += " Content=\"BUTTON\" />";
        }
        
    }

    public ButtonViewModel()
    {
        ReloadButton();
    }
}