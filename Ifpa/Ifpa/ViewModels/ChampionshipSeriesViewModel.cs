using PinballApi.Models.WPPR.v2.Nacs;
using PinballApi.Models.WPPR.v2.Series;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ifpa.ViewModels
{
    public class ChampionshipSeriesViewModel : BaseViewModel
    {
        public ObservableCollection<SeriesOverallResult> SeriesOverallResults { get; set; }
        public Command LoadItemsCommand { get; set; }

        public int Year { get; set; }

        public string SeriesCode { get; set; }
        
        public ChampionshipSeriesViewModel(string code, int year)
        {
            this.Year = year;
            this.SeriesCode = code;

            SeriesOverallResults = new ObservableCollection<SeriesOverallResult>();

            Title = SeriesCode;

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }


        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                SeriesOverallResults.Clear();
                var stateProvinceChampionshipSeries = await PinballRankingApiV2.GetSeriesOverallStanding(SeriesCode, Year);

                if (stateProvinceChampionshipSeries != null)
                {
                    foreach (var item in stateProvinceChampionshipSeries.OverallResults)
                    {
                        SeriesOverallResults.Add(item);
                    }
                }
                
                if(Year != DateTime.Now.Year)
                {
                    Title = $"{SeriesCode} {Year}";
                    OnPropertyChanged("Title");
                }
                else
                {
                    Title = SeriesCode;
                    OnPropertyChanged("Title");
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
