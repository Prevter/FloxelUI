using System;

namespace FloxelLib.MVVM;

/// <summary>
/// Used with the Source Generator to generate the property with the <see cref="BaseViewModel.SetField{T}(ref T, T, string)"/> method.<br/>
/// Can also be used to add extra code to the property.
/// </summary>
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
