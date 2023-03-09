using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Plugin.NFC;

namespace Application.NavigationTree;

public class ConnectPageViewModel : INotifyPropertyChanged
{
    public ICommand StartNfcCommand { get; set; }
    public string Content { get; set; } = "NO DATA";
    public string Status { get; set; } = "STATUS";

    public ConnectPageViewModel()
    {
        StartNfcCommand = new Command(StartNfc);
        CrossNFC.Current.OnMessageReceived += Nfc;
    }

    private void StartNfc()
    {
        CrossNFC.Current.StartListening();
    }

    private void StatusChanged(bool isenabled)
    {
        Status = isenabled ? "READING" : "NO READING";
    }

    private void Nfc(ITagInfo taginfo) 
    {
        Content = taginfo.SerialNumber;
        foreach (var id in taginfo.Identifier)
        {
            Content += $"{id}\n";
        }
        OnPropertyChanged(nameof(Content));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}