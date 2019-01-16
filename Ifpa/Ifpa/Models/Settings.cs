using Xamarin.Essentials;

namespace Ifpa.Models
{
    public static class Settings
    {
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

        public static int MyStatsLastTournamentCount
        {
            get => Preferences.Get("LastTournamentCount", 0);
            set => Preferences.Set("LastTournamentCount", value);
        }

        public static int MyStatsCurrentWpprRank
        {
            get => Preferences.Get("CurrentWpprRank", 0);
            set => Preferences.Set("CurrentWpprRank", value);
        }

        public static void SetMyStatsPlayer(int playerId, int lastTournamentCount, int currentWpprRank)
        {
            MyStatsPlayerId = playerId;
            MyStatsLastTournamentCount = lastTournamentCount;
            MyStatsCurrentWpprRank = currentWpprRank;
        }


    }
}
