using PinballApi.Models.WPPR.v2.Nacs;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ifpa.ViewModels
{
    public class ChampionshipSeriesViewModel : BaseViewModel
    {
        public ObservableCollection<NacsStandings> StateProvinceStandings { get; set; }
        public Command LoadItemsCommand { get; set; }

        private readonly int year;
        
        public ChampionshipSeriesViewModel(int year)
        {
            this.year = year;

            StateProvinceStandings = new ObservableCollection<NacsStandings>();

            Title = "Championship Series";

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
                var stateProvinceChampionshipSeries = await PinballRankingApiV2.GetNacsStandings(year);

                foreach (var item in stateProvinceChampionshipSeries)
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
