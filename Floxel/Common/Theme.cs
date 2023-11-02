using System;
using System.Windows;

namespace FloxelLib.Common;

public abstract class Theme : ResourceDictionary
{
    protected Theme(string name)
    {
        Uri uri = new($"/Floxel;component/Themes/{name}.xaml", UriKind.Relative);
        var resourceDictionary = (ResourceDictionary)Application.LoadComponent(uri);
        MergedDictionaries.Add(resourceDictionary);
    }
}
