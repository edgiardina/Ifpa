using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Ifpa.Models;
using Ifpa.Services;
using PinballApi;
using UIKit;

namespace Ifpa.iOS
{
    public class iOSNotificationService : BaseNotificationService
    { 
        public override void SendNotification(string title, string description)
        {
            // Check for new data, and display it
            var notification = new UILocalNotification();

            // set the fire date (the date time in which it will fire)
            notification.FireDate = NSDate.FromTimeIntervalSinceNow(5);

            // configure the alert
            notification.AlertAction = title;
            notification.AlertBody = description;

            // modify the badge
            notification.ApplicationIconBadgeNumber = 1;

            // set the sound to be the default sound
            notification.SoundName = UILocalNotification.DefaultSoundName;

            // schedule it
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }
    }
}