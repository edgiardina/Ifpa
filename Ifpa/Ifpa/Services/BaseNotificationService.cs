using Ifpa.Models;
using PinballApi;
using Plugin.Badge;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Ifpa.Services
{
    public abstract class BaseNotificationService
    {
        private PinballRankingApi PinballRankingApi => new PinballRankingApi(Constants.IfpaApiKey);

        public readonly string NewTournamentNotificationTitle = "New Tournament Result";
        protected readonly string NewTournamentNotificationDescription = "A new tournament result has been posted to your IFPA profile";

        public readonly string NewRankNotificationTitle = "IFPA Rank Change";
        protected readonly string NewRankNotificationDescription = "Your IFPA rank has changed!";

        public async Task NotifyIfUserHasNewlySubmittedTourneyResults()
        {
            var playerId = Preferences.Get("PlayerId", 0);

            if (playerId > 0)
            {
                try
                {
                    var results = await PinballRankingApi.GetPlayerResults(playerId);

                    var numberOfTournaments = results.ResultsCount;
                    var lastTournamentCount = Preferences.Get("LastTournamentCount", numberOfTournaments);

                    if (numberOfTournaments > lastTournamentCount)
                    {
                        SendNotification(NewTournamentNotificationTitle, NewTournamentNotificationDescription);

                        for (int i = lastTournamentCount; i < numberOfTournaments; i++)
                        {
                            var index = numberOfTournaments - i - 1;

                            var record = new ActivityFeedItem
                            {
                                CreatedDateTime = DateTime.Now,
                                HasBeenSeen = false,
                                RecordID = results.Results[index].TournamentId,
                                IntOne = results.Results[index].Position,
                                ActivityType = ActivityFeedType.TournamentResult
                            };
                            
                            Preferences.Set("LastTournamentCount", numberOfTournaments);

                            await App.ActivityFeed.CreateActivityFeedRecord(record);
                            await UpdateBadgeIfNeeded();
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task NotifyIfUsersRankChanged()
        {
            var playerId = Preferences.Get("PlayerId", 0);

            if (playerId > 0)
            {
                try
                {
                    var results = await PinballRankingApi.GetPlayerRecord(playerId);

                    var currentWpprRank = results.PlayerStats.CurrentWpprRank;
                    var lastRecordedWpprRank = Preferences.Get("CurrentWpprRank", 0);

                    if (currentWpprRank != lastRecordedWpprRank && lastRecordedWpprRank != 0)
                    {
                        SendNotification(NewRankNotificationTitle, NewRankNotificationDescription);

                        var record = new ActivityFeedItem
                        {
                            CreatedDateTime = DateTime.Now,
                            HasBeenSeen = false,
                            IntOne = currentWpprRank,
                            IntTwo = lastRecordedWpprRank,
                            ActivityType = ActivityFeedType.RankChange
                        };
                        
                        Preferences.Set("CurrentWpprRank", currentWpprRank);

                        await App.ActivityFeed.CreateActivityFeedRecord(record);
                        await UpdateBadgeIfNeeded();                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async Task UpdateBadgeIfNeeded()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                var unreads = await App.ActivityFeed.GetUnreadActivityCount();
                CrossBadge.Current.SetBadge(unreads);
            }
        }

        public abstract void SendNotification(string title, string description);
    }
}
