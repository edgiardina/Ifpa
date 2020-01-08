using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Rankings;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomRankingsDetailPage : ContentPage
    {
        CustomRankingsDetailViewModel viewModel;

        public CustomRankingsDetailPage(int viewId)
        {
            InitializeComponent();

            BindingContext = viewModel = new CustomRankingsDetailViewModel(viewId);
        }


        protected override void OnAppearing()
        {
            if (viewModel.ViewResults.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            base.OnAppearing();
        }

        private async void TournamentListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var tournament = e.SelectedItem as Tournament;
            if (tournament == null)
                return;

            await Navigation.PushAsync(new TournamentResultsPage(new TournamentResultsViewModel(tournament.TournamentId)));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void RankingsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var result = e.SelectedItem as CustomRankingViewResult;
            if (result == null)
                return;

            await Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(result.PlayerId)));            

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}