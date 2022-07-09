using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Android.Runtime;
using Plugin.CurrentActivity;
using Microsoft.Maui.Platform.Android.AppLinks;
using Microsoft.Extensions.DependencyInjection;
using Ifpa.Interfaces;
using Ifpa.Droid.Services;
using Plugin.LocalNotification;
using Microsoft.Maui;

namespace Ifpa.Droid
{
    [Activity(Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(new[] { Intent.ActionView },
          Categories = new[] { Intent.CategoryDefault },
          DataScheme = "ifpa",
          DataHost = "ifpacompanion",
          AutoVerify = true)]
    [IntentFilter(new[] { Intent.ActionView },
          Categories = new[] {
              Android.Content.Intent.CategoryDefault,
              Android.Content.Intent.CategoryBrowsable 
          },
          DataSchemes = new string[] { "http", "https" },
          DataHost = "www.ifpapinball.com",
          DataPaths = new string[] { "/player.php", "/tournaments/view.php" },
          AutoVerify = true)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            AndroidAppLinks.Init(this);

            NotificationCenter.CreateNotificationChannel();

            LoadApplication(new App(PlatformSpecificServices));

            NotificationCenter.NotifyNotificationTapped(Intent);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            var alarmIntent = new Intent(this, typeof(BackgroundReceiver));
            var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            alarmManager.SetInexactRepeating(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 3 * 1000, AlarmManager.IntervalFifteenMinutes, pending);
        }

        static void PlatformSpecificServices(IServiceCollection services)
        {
            services.AddSingleton<IReminderService, AndroidReminderService>();
        }

        protected override void OnNewIntent(Intent intent)
        {
            NotificationCenter.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
        }
    }
}