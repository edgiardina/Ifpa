using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui;
using PinballApi.Models.WPPR.v2.Tournaments;
using Ifpa.Models;

namespace Ifpa.ViewModels
{
    public class TournamentResultsViewModel : BaseViewModel
    {
        public ObservableCollectionRange<TournamentResult> Results { get; set; }

        public Tournament TournamentDetails { get; set; }
        public Command LoadItemsCommand { get; set; }

        public int TournamentId { get; set; }

        public TournamentResultsViewModel()
        {
            Title = "Tournament Results";
            Results = new ObservableCollectionRange<TournamentResult>();
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

                Results.AddRange(tournamentResults.Results);           

                Title = TournamentDetails.TournamentName;
                OnPropertyChanged(nameof(TournamentDetails));

                AddTournamentToAppLinks();
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

        private void AddTournamentToAppLinks()
        {
            var url = $"https://www.ifpapinball.com/tournaments/view.php?t={TournamentId}";

            var entry = new AppLinkEntry
            {
                Title = TournamentDetails.TournamentName,
                Description = TournamentDetails.EventName,
                AppLinkUri = new Uri(url, UriKind.RelativeOrAbsolute),
                IsLinkActive = true
                //TODO: show thumbnail?
                //Thumbnail = ImageSource.FromUri(new Uri(TournamentDetails., UriKind.RelativeOrAbsolute))
            };

            entry.KeyValues.Add("contentType", "Tournament Result");
            entry.KeyValues.Add("appName", "IFPA Companion");

            Application.Current.AppLinks.RegisterLink(entry);
        }
    }
}