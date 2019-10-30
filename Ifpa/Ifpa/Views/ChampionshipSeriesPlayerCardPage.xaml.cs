using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Nacs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChampionshipSeriesPlayerCardPage : ContentPage
    {
        ChampionshipSeriesPlayerCardViewModel viewModel;

        public ChampionshipSeriesPlayerCardPage(ChampionshipSeriesPlayerCardViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tournamentCardRecord = e.Item as NacsTournamentCardRecord;
            if (tournamentCardRecord == null)
                return;

            await Navigation.PushAsync(new TournamentResultsPage(new TournamentResultsViewModel(tournamentCardRecord.TournamentId)));

            ////Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.TournamentCardRecords.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }  
    }
}
