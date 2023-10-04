using FloxelLib.MVVM;

namespace FloxelPlayground.ViewModels;

public sealed partial class NumericTextBoxViewModel : BaseViewModel
{
	[UpdateProperty]
	private string _intValue = "10";

	[UpdateProperty]
	private string _doubleValue = "3.14";
}