using Ifpa.Services;
using PinballApi.Models.WPPR.Players;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Ifpa.Models
{
    public static class Settings
    {
        static ActivityFeedDatabase activityFeed;

        public static ActivityFeedDatabase ActivityFeed
        {
            get
            {
                if (activityFeed == null)
                {
                    activityFeed = new ActivityFeedDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ActivityFeedSQLite.db3"));
                }
                return activityFeed;
            }
        }

        public static bool NotifyOnRankChange
        {
            get => Preferences.Get("NotifyOnRankChange", true);
            set => Preferences.Set("NotifyOnRankChange", value);
        }
        public static bool NotifyOnTournamentResult
        {
            get => Preferences.Get("NotifyOnTournamentResult", true);
            set => Preferences.Set("NotifyOnTournamentResult", value);
        }

        public static bool HasConfiguredMyStats
        {
            get => MyStatsPlayerId != 0;
        }

        public static int MyStatsPlayerId
        {
            get => Preferences.Get("PlayerId", 0);
            private set => Preferences.Set("PlayerId", value);
        }

        public static int MyStatsCurrentWpprRank
        {
            get => Preferences.Get("CurrentWpprRank", 0);
            set => Preferences.Set("CurrentWpprRank", value);
        }

        public static void SetMyStatsPlayer(int playerId, int currentWpprRank)
        {
            MyStatsPlayerId = playerId;
            MyStatsCurrentWpprRank = currentWpprRank;
        }

        public static async Task<IEnumerable<int>> FindUnseenTournaments(IList<Result> results)
        {
            return await ActivityFeed.ParseNewTournaments(results.Select(n => n.TournamentId));
        }

    }
}
