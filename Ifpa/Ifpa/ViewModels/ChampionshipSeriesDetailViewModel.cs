using PinballApi.Models.v2.WPPR;
using PinballApi.Models.WPPR.v2.Nacs;
using PinballApi.Models.WPPR.v2.Series;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ifpa.ViewModels
{
    public class ChampionshipSeriesDetailViewModel : BaseViewModel
    {
        public RegionStandings RegionStandings { get; set; }

        public Command LoadItemsCommand { get; set; }

        public readonly string RegionCode;
        public readonly string SeriesCode;
        public readonly int Year;

        public ChampionshipSeriesDetailViewModel(string seriesCode, string regionCode, int year)
        {
            this.SeriesCode = seriesCode;
            this.RegionCode = regionCode;
            this.Year = year;

            RegionStandings = new RegionStandings();

            Title = $"{regionCode} {seriesCode} ({year})";

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }


        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                RegionStandings = await PinballRankingApiV2.GetSeriesStandingsForRegion(SeriesCode, RegionCode, Year);
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
