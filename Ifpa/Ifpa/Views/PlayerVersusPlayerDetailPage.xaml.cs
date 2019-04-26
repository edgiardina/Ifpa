using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v1.Pvp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerVersusPlayerDetailPage : ContentPage
    {
        PlayerVersusPlayerDetailViewModel viewModel;

        public PlayerVersusPlayerDetailPage(PlayerVersusPlayerDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {            
            var tournament = e.Item as Pvp;
            if (tournament == null)
                return;

            await Navigation.PushAsync(new TournamentResultsPage(new TournamentResultsViewModel(int.Parse(tournament.TournamentId))));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.PlayerVersusPlayer.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
