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
        public Command LoadItemsCommand { get; set; }

        private string stateProvinceAbbreviation;

        public ChampionshipSeriesDetailViewModel(string stateProvinceAbbreviation)
        {
            this.stateProvinceAbbreviation = stateProvinceAbbreviation;
            StateProvinceStandings = new ObservableCollection<PlayerStanding>();

            Title = $"{stateProvinceAbbreviation} Standings";

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
                var stateProvinceChampionshipSeries = await PinballRankingApiV2.GetNacsStateProvinceStandings(stateProvinceAbbreviation);

                foreach (var item in stateProvinceChampionshipSeries.PlayerStandings)
                {
                    StateProvinceStandings.Add(item);
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
