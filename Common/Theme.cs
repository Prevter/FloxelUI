using System;
using System.Windows;

namespace FloxelUI.Common;

public class Theme : ResourceDictionary
{
	public Theme(string name)
	{
		Uri uri = new($"/FloxelUI;component/Themes/{name}.xaml", UriKind.Relative);
		ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(uri);
		MergedDictionaries.Add(resourceDictionary);
	}
}
