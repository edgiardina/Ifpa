using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.v2.Tournaments;

namespace Ifpa.ViewModels
{
    public class TournamentResultsViewModel : BaseViewModel
    {
        public ObservableCollection<TournamentResult> Results { get; set; }

        public Tournament TournamentDetails { get; set; }
        public Command LoadItemsCommand { get; set; }

        public int TournamentId { get; set; }

        public TournamentResultsViewModel(int tournamentId)
        {
            Title = "Tournament Results";
            this.TournamentId = tournamentId;
            Results = new ObservableCollection<TournamentResult>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Results.Clear();
                var tournamentResults = await PinballRankingApiV2.GetTournamentResults(TournamentId);
                TournamentDetails = await PinballRankingApiV2.GetTournament(TournamentId);

                foreach (var item in tournamentResults.Results)
                {                 
                    Results.Add(item);
                }

                Title = TournamentDetails.TournamentName;
                OnPropertyChanged(nameof(TournamentDetails));
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