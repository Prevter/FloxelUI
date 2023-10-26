using FloxelLib.MVVM;
using System.Windows;

namespace FloxelPlayground.ViewModels;

public sealed partial class ButtonViewModel : BaseViewModel
{
	private string _buttonVariant = "Contained", _buttonColor = "Primary";

	[UpdateProperty]
	private string _buttonCode = "<Button/>";

	[UpdateProperty("ReloadButton();")]
	private bool _buttonEnabled = true, _buttonIcon = true;

	[UpdateProperty]
	private Style _playgroundButtonStyle;

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
    <StackPanel Orientation=""Horizontal"">
        <fuic:Icon Kind=""Star"" Width=""18"" Margin=""0,0,4,0""/>
        <TextBlock Text=""BUTTON""/>
    </StackPanel>
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