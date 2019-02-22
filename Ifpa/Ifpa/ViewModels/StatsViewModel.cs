using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.Players;
using System.Linq;
using PinballApi.Models.WPPR.Statistics;

namespace Ifpa.ViewModels
{

    public class StatsViewModel : BaseViewModel
    {
        public ObservableCollection<PlayersByCountryStat> PlayersByCountry { get; set; }
        public ObservableCollection<EventsByYearStat> EventsByYear { get; set; }

        public ObservableCollection<PlayersByYearStat> PlayersByYear { get; set; }

        public ObservableCollection<PointsThisYearStat> MostPointsPlayers { get; set; }

        public ObservableCollection<MostEventsStat> MostEventsPlayers { get; set; }

        public ObservableCollection<BiggestMoversStat> BiggestMovers { get; set; }

        public Command LoadItemsCommand { get; set; }

        public StatsViewModel()
        {
            Title = "Stats";
            PlayersByCountry = new ObservableCollection<PlayersByCountryStat>();
            EventsByYear = new ObservableCollection<EventsByYearStat>();
            PlayersByYear = new ObservableCollection<PlayersByYearStat>();
            MostPointsPlayers = new ObservableCollection<PointsThisYearStat>();
            MostEventsPlayers = new ObservableCollection<MostEventsStat>();
            BiggestMovers = new ObservableCollection<BiggestMoversStat>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                PlayersByCountry.Clear();
                EventsByYear.Clear();
                PlayersByYear.Clear();
                MostPointsPlayers.Clear();
                MostEventsPlayers.Clear();
                BiggestMovers.Clear();

                var playersByCountry = await PinballRankingApi.GetPlayersByCountryStat();
                foreach (var item in playersByCountry)
                {
                    PlayersByCountry.Add(item);
                }

                var eventsByYear = await PinballRankingApi.GetEventsPerYearStat();
                foreach (var item in eventsByYear)
                {                    
                    EventsByYear.Add(item);
                }

                var playersByYear = await PinballRankingApi.GetPlayersPerYearStat();
                foreach (var item in playersByYear)
                {
                    PlayersByYear.Add(item);
                }

                var mostPointsPlayers = await PinballRankingApi.GetPointsThisYearStats();
                foreach(var item in mostPointsPlayers)
                {
                    MostPointsPlayers.Add(item);
                }

                var mostEventsPlayers = await PinballRankingApi.GetMostEventsStats();
                foreach(var item in mostEventsPlayers)
                {
                    MostEventsPlayers.Add(item);
                }

                var biggestMovers = await PinballRankingApi.GetBiggestMoversStat();
                foreach(var item in biggestMovers)
                {
                    BiggestMovers.Add(item);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}