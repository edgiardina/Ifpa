using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Series;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChampionshipSeriesDetailPage : ContentPage
    {
        ChampionshipSeriesDetailViewModel viewModel;

        public ChampionshipSeriesDetailPage(ChampionshipSeriesDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var playerStanding = e.Item as RegionStanding;
            if (playerStanding == null)
                return;

            await Navigation.PushAsync(new ChampionshipSeriesPlayerCardPage(new ChampionshipSeriesPlayerCardViewModel(viewModel.Year, playerStanding.PlayerId, viewModel.RegionCode, viewModel.SeriesCode)));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tournament = e.Item as SubmittedTournament;
            if (tournament == null)
                return;

            await Shell.Current.GoToAsync($"tournament-results?tournamentId={tournament.TournamentId}");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
