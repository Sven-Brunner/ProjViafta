using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using Application.Leaderboard;
using Newtonsoft.Json;
using Plugin.NFC;

namespace Application.NavigationTree;

public class ConnectPageViewModel
{
    private const string Password = "GURKENSALAMIAUFLAUF";
    private static readonly Encoding encoding = Encoding.UTF8;

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

    private async void Nfc(ITagInfo taginfo)
    {
        var result = await App.Current.MainPage.DisplayPromptAsync("Werte hochladen", "Bitte geben Sie Ihren Namen ein",
            "Ok", "Abbruch", "Max Muster", -1, Keyboard.Default, string.Empty);
        if (!string.IsNullOrEmpty(result))
        {
            var rnd = new Random();
            var http = new HttpClient();
            var record = new Record(result, rnd.Next(1, 20), rnd.Next(1, 20), DateTime.Now);
            var body = JsonConvert.SerializeObject(record);

            var keyByte = encoding.GetBytes(Password);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                hmacsha256.ComputeHash(encoding.GetBytes(body));
                var hash = ByteToString(hmacsha256.Hash).ToLower();
                http.DefaultRequestHeaders.Add("VERIFICATION_SIGNATURE", hash);
            }


            await http.PostAsync("https://viafta.brainyxs.com", new StringContent(body));
        }
    }

    static string ByteToString(byte[] buff)
    {
        return buff.Aggregate("", (current, t) => current + t.ToString("X2"));
    }
}