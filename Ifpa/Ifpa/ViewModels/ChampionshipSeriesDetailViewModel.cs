using PinballApi.Models.v2.WPPR;
using PinballApi.Models.WPPR.v2.Nacs;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ifpa.ViewModels
{
    public class ChampionshipSeriesDetailViewModel : BaseViewModel
    {
        public ObservableCollection<PlayerStanding> StateProvinceStandings { get; set; }
        public ObservableCollection<NacsStateProvinceStatistics> StateProvinceStatistics { get; set; }
        public ObservableCollection<NacsPayout> StateProvincePayouts { get; set; }
        public Command LoadItemsCommand { get; set; }

        private string stateProvinceAbbreviation;

        public ChampionshipSeriesDetailViewModel(string stateProvinceAbbreviation)
        {
            this.stateProvinceAbbreviation = stateProvinceAbbreviation;
            StateProvinceStandings = new ObservableCollection<PlayerStanding>();
            StateProvinceStatistics = new ObservableCollection<NacsStateProvinceStatistics>();
            StateProvincePayouts = new ObservableCollection<NacsPayout>();

            Title = $"{stateProvinceAbbreviation} Championship Series";

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }


        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                StateProvinceStandings.Clear();
                StateProvinceStatistics.Clear();
                StateProvincePayouts.Clear();

                var stateProvinceChampionshipSeries = await PinballRankingApiV2.GetNacsStateProvinceStandings(stateProvinceAbbreviation);

                foreach (var item in stateProvinceChampionshipSeries.PlayerStandings)
                {
                    StateProvinceStandings.Add(item);
                }

                foreach (var item in stateProvinceChampionshipSeries.Statistics)
                {
                    StateProvinceStatistics.Add(item);
                }

                foreach (var item in stateProvinceChampionshipSeries.Payouts)
                {
                    StateProvincePayouts.Add(item);
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
