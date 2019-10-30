using PinballApi.Models.WPPR.v2.Nacs;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ifpa.ViewModels
{
    public class ChampionshipSeriesPlayerCardViewModel : BaseViewModel
    {
        public ObservableCollection<NacsTournamentCardRecord> TournamentCardRecords { get; set; }
        public Command LoadItemsCommand { get; set; }
        private readonly int year;
        private readonly int playerId;
        private readonly string stateProvinceAbbreviation;


        public ChampionshipSeriesPlayerCardViewModel(int year, int playerId, string stateProvinceAbbreviation)
        {
            this.year = year;
            this.playerId = playerId;
            this.stateProvinceAbbreviation = stateProvinceAbbreviation;

            TournamentCardRecords = new ObservableCollection<NacsTournamentCardRecord>();

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
                TournamentCardRecords.Clear();
                var tournamentCard = await PinballRankingApiV2.GetNacsTournamentCard(year, playerId, stateProvinceAbbreviation);

                foreach (var item in tournamentCard.NacsTournamentCardRecords)
                {
                    TournamentCardRecords.Add(item);
                }

                Title = $"{stateProvinceAbbreviation} Championship Series - {tournamentCard.PlayerName}";
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
