using System.Windows.Input;
using Application.NavigationTree.MainApp.Leaderboard;

namespace Application;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
	}

	public ICommand BackCommand { get; } = new Command(GoBack);

	private static void GoBack()
	{
		Current.GoToAsync("..");
	}
}
