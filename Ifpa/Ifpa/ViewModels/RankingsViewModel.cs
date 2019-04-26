using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.v1.Statistics;
using System.Linq;
using Ifpa.Models;

namespace Ifpa.ViewModels
{
    public class RankingsViewModel : BaseViewModel
    {
        public ObservableCollection<RankingWithFormattedLocation> Players { get; set; }
        public ObservableCollection<PlayersByCountryStat> Countries { get; set; }

        public PlayersByCountryStat CountryToShow { get; set; } 

        public Command LoadItemsCommand { get; set; }

        private int startingPosition;
        public int StartingPosition
        {
            get { return startingPosition; }
            set { SetProperty(ref startingPosition, value); }
        }

        private int countOfItemsToFetch;
        public int CountOfItemsToFetch
        {
            get { return countOfItemsToFetch; }
            set { SetProperty(ref countOfItemsToFetch, value); }
        }

        public readonly PlayersByCountryStat OverallRankings = new PlayersByCountryStat { CountryName = "Overall" };        

        public RankingsViewModel()
        {
            Title = "Rankings";
            CountOfItemsToFetch = 100;
            StartingPosition = 1;
            Players = new ObservableCollection<RankingWithFormattedLocation>();
            Countries = new ObservableCollection<PlayersByCountryStat>();
            CountryToShow = OverallRankings;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (Countries.Count == 0)
                {
                    Countries.Add(OverallRankings);
                    var playersByCountry = await PinballRankingApi.GetPlayersByCountryStat();
                    foreach (var stat in playersByCountry.OrderBy(n => n.CountryName))
                    {
                        Countries.Add(stat);
                    }
                    CountryToShow = Countries.Single(n => n.CountryName == CountryToShow.CountryName);
                    OnPropertyChanged(nameof(CountryToShow));
                }

                Players.Clear();
                var items = await PinballRankingApi.GetRankings(StartingPosition, CountOfItemsToFetch, countryName: CountryToShow.CountryName == OverallRankings.CountryName ? null : CountryToShow?.CountryName);
                foreach (var item in items.Rankings)
                {
                    Players.Add(new RankingWithFormattedLocation(item));
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