using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinballApi.Models.WPPR.Tournaments;

namespace Ifpa.ViewModels
{
    public class TournamentResultsViewModel : BaseViewModel
    {
        public ObservableCollection<Result> Results { get; set; }
        public Command LoadItemsCommand { get; set; }

        private int tournamentId;

        public TournamentResultsViewModel(int tournamentId)
        {
            Title = "Tournament Results";
            this.tournamentId = tournamentId;
            Results = new ObservableCollection<Result>();
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
                var tournamentResults = await PinballRankingApi.GetTournamentResults(tournamentId);

                foreach (var item in tournamentResults.Results)
                {                 
                    Results.Add(item);
                }

                Title = tournamentResults.TournamentName;
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