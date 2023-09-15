using System;
using System.Windows;

namespace FloxelLib;

public partial class Resources
{
	public Resources()
	{
		InitializeComponent();
		Floxel.LoadTheme();
		AddResource("/Floxel;component/Styles/Styles.xaml");
		Floxel.InitApplication();
	}
	
	private void AddResource(string path)
	{
		Uri uri = new(path, UriKind.Relative);
		var resourceDictionary = (ResourceDictionary)Application.LoadComponent(uri);
		MergedDictionaries.Add(resourceDictionary);
	}
}
