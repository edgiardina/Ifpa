using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using Xamarin.Essentials;
using PinballApi.Models.WPPR.v2;
using System;
using PinballApi.Models.WPPR.v2.Rankings;
using System.Linq;
using System.Threading.Tasks;

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

        protected override async void OnAppearing()
        {
            if (viewModel.Players.Count == 0)
            {
                viewModel.CountOfItemsToFetch = Preferences.Get("PlayerCount", viewModel.CountOfItemsToFetch);
                viewModel.StartingPosition = Preferences.Get("StartingRank", viewModel.StartingPosition);
                viewModel.CurrentRankingType = (RankingType)Enum.Parse(typeof(RankingType), Preferences.Get("RankingType", viewModel.CurrentRankingType.ToString()));
                viewModel.CurrentTournamentType = (TournamentType)Enum.Parse(typeof(TournamentType), Preferences.Get("TournamentType", viewModel.CurrentTournamentType.ToString()));

                viewModel.CountryToShow = new Country { CountryName = Preferences.Get("CountryName", viewModel.DefaultCountry.CountryName) };

                await Task.Run(() => viewModel.LoadItemsCommand.Execute(null));
            }
            base.OnAppearing();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var player = args.SelectedItem as RankingResult;
            if (player == null)
                return;

            await Shell.Current.GoToAsync($"player-details?playerId={player.PlayerId}");

            // Manually deselect item.
            PlayersListView.SelectedItem = null;
        }

        private async void InfoButton_Clicked(object sender, EventArgs e)
        {
            var filterPage = new RankingsFilterModalPage(viewModel);
            
            //filterPage.FilterSaved += () => { viewModel.LoadItemsCommand.Execute(null); };

            await Navigation.PushModalAsync(filterPage);
        }

        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("player-search");
        }
    }
}