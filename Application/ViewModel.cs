using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Application;

public class ViewModel
{
    public ISeries[] Series { get; set; } =
    {
        new LineSeries<double>
        {
            Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
            Fill = null
        }
    };

    public ICommand BrCommand { get; set; } = new Command(() =>
    {
        Shell.Current.GoToAsync("//Leaderboard", true).GetAwaiter().GetResult();
    });
}