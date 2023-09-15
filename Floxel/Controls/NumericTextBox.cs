using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FloxelLib.Controls;

public sealed partial class NumericTextBox : TextBox
{
	public NumericTextBox()
	{
		Text = IsDecimal ? Value.ToString(CultureInfo.InvariantCulture) : Math.Floor(Value).ToString(CultureInfo.InvariantCulture);
	}

	public bool IsDecimal
	{
		get => (bool)GetValue(IsDecimalProperty);
		set => SetValue(IsDecimalProperty, value);
	}

	public static readonly DependencyProperty IsDecimalProperty =
		DependencyProperty.Register(nameof(IsDecimal), typeof(bool), typeof(NumericTextBox), new PropertyMetadata(false));
	
	public double Value
	{
		get => (double)GetValue(ValueProperty);
		set
		{
			SetValue(ValueProperty, value);
			Text = IsDecimal ? Value.ToString(CultureInfo.InvariantCulture) : Math.Floor(Value).ToString(CultureInfo.InvariantCulture);
		}
	}
	
	public static readonly DependencyProperty ValueProperty =
		DependencyProperty.Register(nameof(Value), typeof(double), typeof(NumericTextBox), new PropertyMetadata(0.0));
	
	public double MinValue
	{
		get => (double)GetValue(MinValueProperty);
		set => SetValue(MinValueProperty, value);
	}
	
	public static readonly DependencyProperty MinValueProperty =
		DependencyProperty.Register(nameof(MinValue), typeof(double), typeof(NumericTextBox), new PropertyMetadata(double.MinValue));
	
	public double MaxValue
	{
		get => (double)GetValue(MaxValueProperty);
		set => SetValue(MaxValueProperty, value);
	}
	
	public static readonly DependencyProperty MaxValueProperty =
		DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(NumericTextBox), new PropertyMetadata(double.MaxValue));


	protected override void OnPreviewTextInput(TextCompositionEventArgs e)
	{
		e.Handled = !AreAllValidNumericChars(e.Text);
		if (e.Handled && Text.Parse(out double value) && value >= MinValue && value <= MaxValue)
			Value = value;
		
		base.OnPreviewTextInput(e);
	}

	/// <summary>
	/// To check if numbers entered are all valid numeric numbers
	/// </summary>
	private bool AreAllValidNumericChars(string str)
	{
		var ret = true;
		if (str == "-" | str == "+")
			return ret;

		if (IsDecimal && str == ".") return ret;

		int l = str.Length;
		for (var i = 0; i < l; i++)
		{
			char ch = str[i];
			ret &= char.IsDigit(ch);
		}

		return ret;
	}
}