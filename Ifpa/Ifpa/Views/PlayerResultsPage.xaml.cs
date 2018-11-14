using Ifpa.ViewModels;
using PinballApi.Models.WPPR.Players;
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
            var tournament = e.Item as Result;
            if (tournament == null)
                return;

            await Navigation.PushAsync(new TournamentResultsPage(new TournamentResultsViewModel(tournament.TournamentId)));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Results.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }


        private void ActiveButton_Clicked(object sender, System.EventArgs e)
        {
            viewModel.State = PlayerResultState.Active;
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void PastButton_Clicked(object sender, System.EventArgs e)
        {
            viewModel.State = PlayerResultState.Inactive;
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void UnusedButton_Clicked(object sender, System.EventArgs e)
        {
            viewModel.State = PlayerResultState.NonActive;
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
