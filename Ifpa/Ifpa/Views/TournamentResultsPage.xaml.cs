using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Tournaments;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TournamentResultsPage : ContentPage
    {
        TournamentResultsViewModel viewModel;

        public TournamentResultsPage(TournamentResultsViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            var tournament = e.Item as TournamentResult;
            if (tournament == null)
                return;

            if (tournament.PlayerId.HasValue)
            {
                await Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(tournament.PlayerId.Value)));
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Results.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void ShareButton_Clicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = $"https://www.ifpapinball.com/tournaments/view.php?t={viewModel.TournamentId}",
                Title = "Share Tournament Results"
            });
        }

        private async void InfoButton_Clicked(object sender, EventArgs e)
        {
            var infoPage = new TournamentInfoPage(viewModel);

            await Navigation.PushModalAsync(infoPage);
        }

    }
}
