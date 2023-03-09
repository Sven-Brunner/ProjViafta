using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Application.Leaderboard;
using Newtonsoft.Json;

namespace Application.NavigationTree;

public class LeaderboardViewModel : INotifyPropertyChanged
{
    private bool _isBusy;
    public ObservableCollection<Record> Records { get; set; }
    public ICommand InitializeAsyncCommand { get; set; }

    public LeaderboardViewModel()
    {
        InitializeAsyncCommand = new Command(InitRecords);
        IsBusy = true;
        Records = new ObservableCollection<Record>();
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (value == _isBusy) return;
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    private async void InitRecords()
    {
        IsBusy = true;
        Records.Clear();
        using (var client = new HttpClient())
        {
            var json = await client.GetStringAsync("https://viafta.brainyxs.com?");
            var array = JsonConvert.DeserializeObject<List<Record>>(json);
            foreach (var record in array)
            {
                Records.Add(record);
            }

            IsBusy = false;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}