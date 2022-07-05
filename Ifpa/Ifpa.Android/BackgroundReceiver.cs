using Android.Content;
using Android.OS;
using Ifpa.Droid.Services;
using Ifpa.Services;

namespace Ifpa.Droid
{
    [BroadcastReceiver]
    public class BackgroundReceiver : BroadcastReceiver
    {
        private NotificationService NotificationService;

        public override async void OnReceive(Context context, Intent intent)
        {
            PowerManager pm = (PowerManager)context.GetSystemService(Context.PowerService);
            PowerManager.WakeLock wakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "BackgroundReceiver");
            wakeLock.Acquire();

            NotificationService = new NotificationService();

            await NotificationService.NotifyIfUserHasNewlySubmittedTourneyResults();
            await NotificationService.NotifyIfUsersRankChanged();
            await NotificationService.NotifyIfNewBlogItemPosted();

            wakeLock.Release();
        }
    }
}