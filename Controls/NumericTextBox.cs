using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FloxelUI.Controls;

public sealed partial class NumericTextBox : TextBox
{
	public NumericTextBox()
	{
		Text = "0";
	}

	public bool IsDecimal
	{
		get { return (bool)GetValue(IsDecimalProperty); }
		set { SetValue(IsDecimalProperty, value); }
	}

	public static readonly DependencyProperty IsDecimalProperty =
		DependencyProperty.Register("IsDecimal", typeof(bool), typeof(NumericTextBox), new PropertyMetadata(true));

	protected override void OnPreviewTextInput(TextCompositionEventArgs e)
	{
		e.Handled = !AreAllValidNumericChars(e.Text);
		base.OnPreviewTextInput(e);
	}

	/// <summary>
	/// To check if numbers entered are all valid numeric numbers
	/// </summary>
	private bool AreAllValidNumericChars(string str)
	{
		bool ret = true;
		if (str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeSign |
			str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveSign)
			return ret;

		if (IsDecimal && str == ".") return ret;

		int l = str.Length;
		for (int i = 0; i < l; i++)
		{
			char ch = str[i];
			ret &= char.IsDigit(ch);
		}

		return ret;
	}
}