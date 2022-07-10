using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Tournaments;
using System;
using Microsoft.Maui;

namespace Ifpa.Views
{
    [QueryProperty("TournamentId", "tournamentId")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TournamentResultsPage : ContentPage
    {
        TournamentResultsViewModel viewModel;
        
        public int TournamentId { get; set; }

        public TournamentResultsPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = App.GetViewModel<TournamentResultsViewModel>(); ;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            var tournament = e.Item as TournamentResult;
            if (tournament == null)
                return;

            if (tournament.PlayerId.HasValue)
            {
                await Shell.Current.GoToAsync($"player-details?playerId={tournament.PlayerId.Value}");
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Results.Count == 0)
            {
                viewModel.TournamentId = TournamentId;
                viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private async void ShareButton_Clicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = $"https://www.ifpapinball.com/tournaments/view.php?t={viewModel.TournamentId}",
                Title = "Share Tournament Results"
            });
        }

        private async void InfoButton_Clicked(object sender, EventArgs e)
        {
            var infoPage = new TournamentInfoPage(viewModel);

            await Navigation.PushModalAsync(infoPage);
        }

    }
}
