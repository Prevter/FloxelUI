using System.Windows;
using FloxelLib;
using FloxelLib.MVVM;

namespace FloxelPlayground.ViewModels;

public sealed partial class NumericTextBoxViewModel : BaseViewModel
{
	private string _intValue = "10";
	public string IntValue
	{
		get => _intValue;
		set => SetField(ref _intValue, value);
	}

	private string _doubleValue = "3.14";
	public string DoubleValue
	{
		get => _doubleValue;
		set => SetField(ref _doubleValue, value);
	}
}