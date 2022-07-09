using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Players;
using Microsoft.Maui;
using Microsoft.Maui.Xaml;

namespace Ifpa.Views
{
    [QueryProperty("PlayerId", "playerId")]
    [QueryProperty("ComparePlayerId", "comparePlayerId")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerVersusPlayerDetailPage : ContentPage
    {
        PlayerVersusPlayerDetailViewModel viewModel;

        public int PlayerId { get; set; }

        public int ComparePlayerId { get; set; }

        public PlayerVersusPlayerDetailPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = App.GetViewModel<PlayerVersusPlayerDetailViewModel>(); 
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {            
            var tournament = e.Item as PlayerVersusPlayerComparisonRecord;
            if (tournament == null)
                return;

            await Shell.Current.GoToAsync($"tournament-results?tournamentId={tournament.TournamentId}");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.PlayerVersusPlayer.Count == 0)
            {
                viewModel.PlayerOneId = PlayerId;
                viewModel.PlayerTwoId = ComparePlayerId;
                viewModel.LoadItemsCommand.Execute(null);
            }
        }
    }
}
