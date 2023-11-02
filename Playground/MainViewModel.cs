using FloxelLib;
using FloxelLib.Controls;
using FloxelLib.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace FloxelPlayground;

public sealed partial class MainViewModel : BaseViewModel
{
    private string _darkModeToggleIcon = Floxel.IsDarkMode ? "WhiteBalanceSunny" : "MoonWaningCrescent";
    public string DarkModeToggleIcon
    {
        get => _darkModeToggleIcon;
        set => SetField(ref _darkModeToggleIcon, value);
    }

    [RelayCommand]
    private void ToggleDarkMode()
    {
        if (Floxel.IsDarkMode)
        {
            DarkModeToggleIcon = "MoonWaningCrescent";
            Floxel.SetTheme(Floxel.LightTheme);
        }
        else if (Floxel.IsLightMode)
        {
            DarkModeToggleIcon = "WhiteBalanceSunny";
            Floxel.SetTheme(Floxel.DarkTheme);
        }
    }

    public ObservableCollection<PageOption> Pages { get; set; } = new();

    private int _selectedPageIndex = 0;
    public int SelectedPageIndex
    {
        get => _selectedPageIndex;
        set
        {
            SetField(ref _selectedPageIndex, value);
            SelectedPageUri = Pages[value].Uri;
        }
    }

    private string _selectedPageUri = "";
    public string SelectedPageUri
    {
        get => _selectedPageUri;
        set => SetField(ref _selectedPageUri, value);
    }

    public MainViewModel()
    {
        string pagesNamespace = typeof(App).Namespace + ".Pages";

        var pageTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.Namespace == pagesNamespace && t.IsSubclassOf(typeof(Page)));

        foreach (var pageType in pageTypes)
        {
            var page = (Page?)Activator.CreateInstance(pageType);
            if (page is null) continue;

            Pages.Add(new(page.Title, $"Pages/{pageType.Name}.xaml"));
        }

        SelectedPageUri = Pages[0].Uri;
    }

    public sealed class PageOption
    {
        public string Title { get; set; } = "";
        public string Uri { get; set; } = "";
        public override string ToString() => Title;

        public PageOption(string title, string uri)
        {
            Title = title;
            Uri = uri;
        }
    }
}