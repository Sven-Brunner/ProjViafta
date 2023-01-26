using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Application.NavigationTree.MainApp;

public class MainPageViewModel
{
    public ICommand BrCommand { get; set; } = new Command(() =>
    {
        Shell.Current.GoToAsync("///Leaderboard", true).GetAwaiter().GetResult();
    });
}