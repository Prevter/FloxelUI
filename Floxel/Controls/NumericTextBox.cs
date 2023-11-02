using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FloxelLib.Controls;

public sealed partial class NumericTextBox : TextBox
{
    public NumericTextBox() { }

    public bool IsDecimal
    {
        get => (bool)GetValue(IsDecimalProperty);
        set => SetValue(IsDecimalProperty, value);
    }

    public static readonly DependencyProperty IsDecimalProperty =
        DependencyProperty.Register(nameof(IsDecimal), typeof(bool), typeof(NumericTextBox), new PropertyMetadata(false));

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