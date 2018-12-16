using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using PinballApi.Models.WPPR.Rankings;
using Xamarin.Essentials;
using PinballApi.Models.WPPR.Statistics;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RankingsPage : ContentPage
    {
        RankingsViewModel viewModel;

        public RankingsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RankingsViewModel();
            PlayerListViewIndexConverter.BindingContext = viewModel;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var player = args.SelectedItem as Ranking;
            if (player == null)
                return;

            await Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(player.PlayerId)));

            // Manually deselect item.
            PlayersListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Players.Count == 0)
            {
                viewModel.CountOfItemsToFetch = Preferences.Get("PlayerCount", viewModel.CountOfItemsToFetch);
                viewModel.StartingPosition = Preferences.Get("StartingRank", viewModel.StartingPosition);
                viewModel.CountryToShow = new PlayersByCountryStat { CountryName = Preferences.Get("CountryName", viewModel.OverallRankings.CountryName) };

                viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private void Picker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Preferences.Set("CountryName", viewModel.CountryToShow.CountryName);
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (viewModel.Players.Count > 0)
            {
                Preferences.Set("PlayerCount", viewModel.CountOfItemsToFetch);
                Preferences.Set("StartingRank", viewModel.StartingPosition);
            }
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            FilterTable.IsVisible = !FilterTable.IsVisible;
        }
    }
}