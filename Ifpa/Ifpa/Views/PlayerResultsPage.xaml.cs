using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2;
using PinballApi.Models.WPPR.v2.Players;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Xaml;

namespace Ifpa.Views
{
    [QueryProperty("PlayerId", "playerId")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerResultsPage : ContentPage
    {
        PlayerResultsViewModel viewModel;
        public int PlayerId { get; set; }
        public ObservableCollection<string> Items { get; set; }

        public PlayerResultsPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = App.GetViewModel<PlayerResultsViewModel>();            
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tournament = e.Item as PlayerResult;
            if (tournament == null)
                return;

            await Shell.Current.GoToAsync($"tournament-results?tournamentId={tournament.TournamentId}");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.ActiveResults.Count == 0)
            {
                viewModel.PlayerId = PlayerId;
                viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private async void RankingProfileButton_Clicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Ranking Profile", "Cancel", null, viewModel.RankingTypeOptions.Select(a => a.ToString()).ToArray());
            
            if (action != "Cancel")
            {
                viewModel.RankingType = (RankingType)Enum.Parse(typeof(RankingType), action);
                viewModel.LoadItemsCommand.Execute(null);
            }
        }
    }
}
