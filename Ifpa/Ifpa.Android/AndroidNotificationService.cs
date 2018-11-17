using Android.App;
using Android.Content;
using Android.OS;
using Ifpa.Services;
using Java.Lang;

namespace Ifpa.Droid
{
    public class AndroidNotificationService : BaseNotificationService
    {
        static readonly int NOTIFICATION_ID = 1000;
        static readonly string CHANNEL_ID = "location_notification";
        internal static readonly string COUNT_KEY = "count";

        private readonly Context context;

        public AndroidNotificationService(Context context)
        {
            this.context = context;
            CreateNotificationChannel();
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = context.GetString(Resource.String.channel_name);
            var channelDescription = context.GetString(Resource.String.channel_description);
            var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public override void SendNotification()
        {
            // When the user clicks the notification, SecondActivity will start up.
            var resultIntent = new Intent(context, typeof(MainActivity));
            
            // Construct a back stack for cross-task navigation:
            var stackBuilder = TaskStackBuilder.Create(context);
            stackBuilder.AddParentStack(Class.FromType(typeof(MainActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:
            var resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);


            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(context, CHANNEL_ID)
                                                        .SetContentTitle(NotificationTitle)
                                                        .SetContentText(NotificationDescription)                                                        
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