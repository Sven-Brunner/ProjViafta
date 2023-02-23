using System.Collections.ObjectModel;
using Application.Leaderboard;
using Newtonsoft.Json;

namespace Application.NavigationTree;

public class LeaderboardViewModel
{
    public ObservableCollection<Record> Records { get; set; }

    public LeaderboardViewModel()
    {
        IsBusy = true;
        Records = new ObservableCollection<Record>();
        InitRecords();
    }

    public bool IsBusy { get; set; }

    private async void InitRecords()
    {
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
}