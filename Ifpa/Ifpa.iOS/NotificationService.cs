using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Ifpa.Models;
using PinballApi;
using UIKit;

namespace Ifpa.iOS
{
    public static class NotificationService
    {
        private static PinballRankingApi PinballRankingApi => new PinballRankingApi(Constants.IfpaApiKey);

        public static async Task NotifyIfUserHasNewlySubmittedTourneyResults()
        {
            var plist = NSUserDefaults.StandardUserDefaults;
            var playerId = Convert.ToInt32(plist.IntForKey("PlayerId"));

            if (playerId > 0)
            {
                var needsNotification = false;

                try
                {
                    var results = await PinballRankingApi.GetPlayerResults(playerId);
                
                    var latestTournamentPosted = results.Results.Max(n => n.TournamentId);
                    var lastTournamentChecked = plist.IntForKey("LastTournamentId");

                    if (latestTournamentPosted > lastTournamentChecked)
                    {
                        plist.SetInt(latestTournamentPosted, "LastTournamentId");
                        needsNotification = true;
                    }

                    if (needsNotification)
                    {
                        // Check for new data, and display it
                        var notification = new UILocalNotification();

                        // set the fire date (the date time in which it will fire)
                        notification.FireDate = NSDate.FromTimeIntervalSinceNow(5);

                        // configure the alert
                        notification.AlertAction = "New Tournament Result";
                        notification.AlertBody = "A new tournament result has been posted to your IFPA profile";
                    
                        // modify the badge
                        notification.ApplicationIconBadgeNumber = 1;

                        // set the sound to be the default sound
                        notification.SoundName = UILocalNotification.DefaultSoundName;

                        // schedule it
                        UIApplication.SharedApplication.ScheduleLocalNotification(notification);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}