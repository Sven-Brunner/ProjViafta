using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Nfc;
using Android.Nfc.Tech;
using Android.OS;
using Plugin.NFC;

namespace Application;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
    {
        CrossNFC.Init(this);
        base.OnCreate(savedInstanceState, persistentState);
    }

    protected override void OnResume()
    {
        base.OnResume();
        CrossNFC.Init(this);
        CrossNFC.OnResume();
    }

    protected override void OnNewIntent(Intent intent)
    {
        base.OnNewIntent(intent);
        if (intent == null || !(intent.Action == "android.nfc.action.TAG_DISCOVERED") &&
            !(intent.Action == "android.nfc.action.NDEF_DISCOVERED"))
            return;
        var tag = intent.GetParcelableExtra("android.nfc.extra.TAG") as Tag;

        if (tag == null) return;

        MifareClassic card = MifareClassic.Get(tag);
        card.Connect();
        //read block 1 of sector 0
        if (card.AuthenticateSectorWithKeyA(0,
                new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }) &&
            card.AuthenticateSectorWithKeyB(0,
                new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })) //replace if you have other auth keys
        {
            for (var i = 0; i < 16; i++)
            {
                try
                {
                    var block = card.SectorToBlock(0);
                    byte[] data = card.ReadBlock(block);
                    var f = data;
                }
                catch (Exception e)
                {
                    //Ignored
                }
            }
        }

        card.Close();
    }
}