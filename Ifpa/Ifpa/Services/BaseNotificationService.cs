using Ifpa.Models;
using PinballApi;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

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

                    var latestTournamentPosted = results.Results.Max(n => n.TournamentId);
                    var lastTournamentChecked = Preferences.Get("LastTournamentId", latestTournamentPosted);

                    if (latestTournamentPosted > lastTournamentChecked)
                    {
                        SendNotification(NewTournamentNotificationTitle, NewTournamentNotificationDescription);
                    }
                    
                    Preferences.Set("LastTournamentId", latestTournamentPosted);
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
                    }

                    Preferences.Set("CurrentWpprRank", currentWpprRank);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public abstract void SendNotification(string title, string description);
    }
}
