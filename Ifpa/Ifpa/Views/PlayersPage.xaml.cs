using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using PinballApi.Models.WPPR.Rankings;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayersPage : ContentPage
    {
        PlayersViewModel viewModel;

        public PlayersPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new PlayersViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var player = args.SelectedItem as Ranking;
            if (player == null)
                return;

            var playerData = await viewModel.PinballRankingApi.GetPlayerRecord(player.PlayerId);

            await Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(playerData)));

            // Manually deselect item.
            PlayersListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Players.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}