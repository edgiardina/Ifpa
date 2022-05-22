using Foundation;
using Ifpa.Services;
using UIKit;

namespace Ifpa.iOS.Services
{
    public class iOSNotificationService : BaseNotificationService
    { 
        public override void SendNotification(string title, string description, string url)
        {
            // Check for new data, and display it
            var notification = new UILocalNotification();

            // set the fire date (the date time in which it will fire)
            notification.FireDate = NSDate.FromTimeIntervalSinceNow(5);

            // configure the alert
            notification.AlertAction = title;
            notification.AlertBody = description;
            

            // set the sound to be the default sound
            notification.SoundName = UILocalNotification.DefaultSoundName;

            // schedule it
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }
    }
}