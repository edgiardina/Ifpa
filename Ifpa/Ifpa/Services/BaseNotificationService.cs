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

        public readonly string NotificationTitle = "New Tournament Result";
        protected readonly string NotificationDescription = "A new tournament result has been posted to your IFPA profile";

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
                        Preferences.Set("LastTournamentId", latestTournamentPosted);
                        SendNotification();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public abstract void SendNotification();
    }
}
