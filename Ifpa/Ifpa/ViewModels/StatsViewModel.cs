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

        public Command LoadItemsCommand { get; set; }

        public StatsViewModel()
        {
            Title = "Stats";
            PlayersByCountry = new ObservableCollection<PlayersByCountryStat>();
            EventsByYear = new ObservableCollection<EventsByYearStat>();
            PlayersByYear = new ObservableCollection<PlayersByYearStat>();

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