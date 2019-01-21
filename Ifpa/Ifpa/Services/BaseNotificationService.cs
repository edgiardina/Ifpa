using Ifpa.Models;
using PinballApi;
using Plugin.Badge;
using System;
using System.Linq;
using System.Threading.Tasks;
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
            if (Settings.HasConfiguredMyStats)
            {
                try
                {
                    var results = await PinballRankingApi.GetPlayerResults(Settings.MyStatsPlayerId);

                    var numberOfTournaments = results.ResultsCount;
                    var lastTournamentCount = Settings.MyStatsLastTournamentCount != 0 ? Settings.MyStatsLastTournamentCount : numberOfTournaments;

                    if (numberOfTournaments > lastTournamentCount)
                    {
                        if (Settings.NotifyOnTournamentResult)
                        {
                            SendNotification(NewTournamentNotificationTitle, NewTournamentNotificationDescription);
                        }

                        var unseenTournaments = await Settings.FindUnseenTournaments(results.Results);

                        foreach (var unseenTournament in unseenTournaments.OrderBy(n => n))
                        {
                            var result = results.Results.Single(n => n.TournamentId == unseenTournament);

                            var record = new ActivityFeedItem
                            {
                                CreatedDateTime = DateTime.Now,
                                HasBeenSeen = false,
                                RecordID = result.TournamentId,
                                IntOne = result.Position,
                                ActivityType = ActivityFeedType.TournamentResult
                            };

                            Settings.MyStatsLastTournamentCount = numberOfTournaments;

                            await Settings.ActivityFeed.CreateActivityFeedRecord(record);

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
            if (Settings.HasConfiguredMyStats)
            {
                try
                {
                    var results = await PinballRankingApi.GetPlayerRecord(Settings.MyStatsPlayerId);

                    var currentWpprRank = results.PlayerStats.CurrentWpprRank;
                    var lastRecordedWpprRank = Settings.MyStatsCurrentWpprRank;

                    if (currentWpprRank != lastRecordedWpprRank && lastRecordedWpprRank != 0)
                    {
                        if (Settings.NotifyOnRankChange)
                        {
                            SendNotification(NewRankNotificationTitle, NewRankNotificationDescription);
                        }

                        var record = new ActivityFeedItem
                        {
                            CreatedDateTime = DateTime.Now,
                            HasBeenSeen = false,
                            IntOne = currentWpprRank,
                            IntTwo = lastRecordedWpprRank,
                            ActivityType = ActivityFeedType.RankChange
                        };

                        Settings.MyStatsCurrentWpprRank = currentWpprRank;

                        await Settings.ActivityFeed.CreateActivityFeedRecord(record);

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
                var unreads = await Settings.ActivityFeed.GetUnreadActivityCount();
                CrossBadge.Current.SetBadge(unreads);
            }
        }

        public abstract void SendNotification(string title, string description);
    }
}
