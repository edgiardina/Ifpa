using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Series;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{

    [QueryProperty("SeriesCode", "seriesCode")]
    [QueryProperty("RegionCode", "regionCode")]
    [QueryProperty("Year", "year")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChampionshipSeriesDetailPage : ContentPage
    {
        ChampionshipSeriesDetailViewModel viewModel;

        public string SeriesCode { get; set; }
        public string RegionCode { get; set; }  
        public int Year { get; set; }


        public ChampionshipSeriesDetailPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = App.GetViewModel<ChampionshipSeriesDetailViewModel>();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var playerStanding = e.Item as RegionStanding;
            if (playerStanding == null)
                return;

            await Shell.Current.GoToAsync($"champ-series-player??seriesCode={SeriesCode}&regionCode={RegionCode}&year={Year}&playerId={playerStanding.PlayerId}");
         
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.Year = Year;
            viewModel.RegionCode = RegionCode;
            viewModel.SeriesCode = SeriesCode;

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
