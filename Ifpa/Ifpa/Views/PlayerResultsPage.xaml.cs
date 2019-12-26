using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2;
using PinballApi.Models.WPPR.v2.Players;
using System;
using System.Collections.ObjectModel;
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

        private void RankingProfileButton_Clicked(object sender, EventArgs e)
        {
            popupLayout.Show();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null && viewModel.RankingType != (RankingType)Enum.Parse(typeof(RankingType), e.SelectedItem.ToString()))
            {
                viewModel.RankingType = (RankingType)Enum.Parse(typeof(RankingType), e.SelectedItem.ToString());           
                viewModel.LoadItemsCommand.Execute(null);
                popupLayout.IsOpen = false;
            }
        }
    }
}
