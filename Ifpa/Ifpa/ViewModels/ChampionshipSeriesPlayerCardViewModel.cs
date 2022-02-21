using PinballApi.Models.WPPR.v2.Nacs;
using PinballApi.Models.WPPR.v2.Series;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ifpa.ViewModels
{
    public class ChampionshipSeriesPlayerCardViewModel : BaseViewModel
    {
        public ObservableCollection<PlayerCard> TournamentCardRecords { get; set; }
        public Command LoadItemsCommand { get; set; }
        private readonly int year;
        private readonly int playerId;
        private readonly string regionCode;
        private readonly string seriesCode;


        public ChampionshipSeriesPlayerCardViewModel(int year, int playerId, string regionCode, string seriesCode)
        {
            this.year = year;
            this.playerId = playerId;
            this.regionCode = regionCode;
            this.seriesCode = seriesCode;

            TournamentCardRecords = new ObservableCollection<PlayerCard>();

            Title = $"{regionCode} Championship Series";

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }


        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                TournamentCardRecords.Clear();
                var tournamentCard = await PinballRankingApiV2.GetSeriesPlayerCard(playerId, seriesCode, regionCode, year);

                foreach (var item in tournamentCard.PlayerCard)
                {
                    TournamentCardRecords.Add(item);
                }

                Title = $"{regionCode} {seriesCode} ({year}) - {tournamentCard.PlayerName}";
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
