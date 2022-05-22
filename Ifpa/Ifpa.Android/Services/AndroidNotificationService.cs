using Android.App;
using Android.Content;
using Android.OS;
using Ifpa.Services;
using Java.Lang;

namespace Ifpa.Droid.Services
{
    public class AndroidNotificationService : BaseNotificationService
    {
        static readonly string TOURNAMENT_CHANNEL_ID = "tournament_notification";
        static readonly string RANK_CHANNEL_ID = "rank_notification";
        internal static readonly string COUNT_KEY = "count";

        private readonly Context context;

        public AndroidNotificationService(Context context)
        {
            this.context = context;
            CreateNotificationChannels();
        }

        void CreateNotificationChannels()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = context.GetString(Resource.String.tournament_channel_name);
            var channelDescription = context.GetString(Resource.String.tournament_channel_description);
            var channel = new NotificationChannel(TOURNAMENT_CHANNEL_ID, channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var channelName2 = context.GetString(Resource.String.rank_channel_name);
            var channelDescription2 = context.GetString(Resource.String.rank_channel_description);
            var channel2 = new NotificationChannel(RANK_CHANNEL_ID, channelName2, NotificationImportance.Default)
            {
                Description = channelDescription2
            };

            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
            notificationManager.CreateNotificationChannel(channel2);
        }

        public override void SendNotification(string title, string description, string url)
        {
            // When the user clicks the notification, SecondActivity will start up.
            var resultIntent = new Intent(context, typeof(MainActivity));

            resultIntent.SetAction("IfpaNotification");
            resultIntent.PutExtra("IfpaShellRoute", url);

            // Create the PendingIntent with the back stack:
            var resultPendingIntent = PendingIntent.GetActivity(context, 0, resultIntent, PendingIntentFlags.CancelCurrent);


            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(context, TOURNAMENT_CHANNEL_ID)
                                                        .SetContentTitle(title)
                                                        .SetContentText(description)                                                        
                                                        .SetSmallIcon(Resource.Drawable.notification_icon)
                                                        .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                                                        .SetContentIntent(resultPendingIntent); 

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                context.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }
    }
}