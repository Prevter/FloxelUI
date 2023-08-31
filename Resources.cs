using System;
using System.Windows;

namespace FloxelUI;

public class Resources : ResourceDictionary
{
	public Resources()
	{
		AddResource("/FloxelUI;component/Resources.xaml");
		AddResource("/FloxelUI;component/Styles/Styles.xaml");
	}

	public void AddResource(string path)
	{
		Uri uri = new(path, UriKind.Relative);
		ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(uri);
		MergedDictionaries.Add(resourceDictionary);
	}
}
