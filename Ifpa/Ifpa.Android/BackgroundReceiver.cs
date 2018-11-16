using Android.Content;
using Android.OS;

namespace Ifpa.Droid
{
    [BroadcastReceiver]
    public class BackgroundReceiver : BroadcastReceiver
    {
        private AndroidNotificationService NotificationService = new AndroidNotificationService();

        public override async void OnReceive(Context context, Intent intent)
        {
            PowerManager pm = (PowerManager)context.GetSystemService(Context.PowerService);
            PowerManager.WakeLock wakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "BackgroundReceiver");
            wakeLock.Acquire();

            await NotificationService.NotifyIfUserHasNewlySubmittedTourneyResults();

            wakeLock.Release();
        }
    }
}