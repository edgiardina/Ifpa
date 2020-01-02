using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2;
using PinballApi.Models.WPPR.v2.Players;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerResultsPage : ContentPage
    {
        PlayerResultsViewModel viewModel;
        public ObservableCollection<string> Items { get; set; }

        public PlayerResultsPage(PlayerResultsViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tournament = e.Item as PlayerResult;
            if (tournament == null)
                return;

            await Navigation.PushAsync(new TournamentResultsPage(new TournamentResultsViewModel(tournament.TournamentId)));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.ActiveResults.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
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
