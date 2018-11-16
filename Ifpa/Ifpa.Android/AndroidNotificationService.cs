using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Ifpa.Services;

namespace Ifpa.Droid
{
    public class AndroidNotificationService : BaseNotificationService
    {
        static readonly int NOTIFICATION_ID = 1000;
        static readonly string CHANNEL_ID = "location_notification";
        internal static readonly string COUNT_KEY = "count";

        public AndroidNotificationService()
        {
            CreateNotificationChannel();
        }

        void CreateNotificationChannel()
        {
            //if (Build.VERSION.SdkInt < BuildVersionCodes.)
            //{
            //    // Notification channels are new in API 26 (and not a part of the
            //    // support library). There is no need to create a notification
            //    // channel on older versions of Android.
            //    return;
            //}

            //var channelName = Resources.GetString(Resource.String.channel_name);
            //var channelDescription = Resources.GetString(Resource.String.channel_description);
            //var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.Default)
            //{
            //    Description = channelDescription
            //};

            //var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            //notificationManager.CreateNotificationChannel(channel);
        }

        public override void SendNotification()
        {
            //// Instantiate the builder and set notification elements:
            //NotificationCompat.Builder builder = new Android.Support.V7.App.NotificationCompat.Builder(this, CHANNEL_ID)
            //                                            .SetContentTitle(NotificationTitle)
            //                                            .SetContentText(NotificationDescription)
            //                                            .SetSmallIcon(Resource.Drawable.cast_ic_notification_0);

            //// Build the notification:
            //Notification notification = builder.Build();

            //// Get the notification manager:
            //NotificationManager notificationManager =
            //    GetSystemService(Context.NotificationService) as NotificationManager;

            //// Publish the notification:
            //const int notificationId = 0;
            //notificationManager.Notify(notificationId, notification);
        }
    }
}