using System;

namespace FloxelLib.MVVM;

[AttributeUsage(AttributeTargets.Field)]
public sealed class UpdateProperty : Attribute
{
	public string? ExtraCode { get; }

	public UpdateProperty() { }

	public UpdateProperty(string extraCode)
	{
		ExtraCode = extraCode;
	}
}
