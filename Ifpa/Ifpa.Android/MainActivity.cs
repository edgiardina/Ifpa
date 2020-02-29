using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android;
using Android.Content;
using Android.Runtime;
using Plugin.CurrentActivity;
using Plugin.AppShortcuts;

namespace Ifpa.Droid
{
    [Activity(Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionView },
          Categories = new[] { Intent.CategoryDefault },
          DataScheme = "ifpa",
          DataHost = "ifpacompanion",
          AutoVerify = true)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            LoadApplication(new App());

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            var alarmIntent = new Intent(this, typeof(BackgroundReceiver));
            var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            alarmManager.SetInexactRepeating(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 3 * 1000, AlarmManager.IntervalFifteenMinutes, pending);
        }
    }
}