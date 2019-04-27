using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v1.Players;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerChampionshipSeriesPage : ContentPage
    {
        PlayerChampionshipSeriesViewModel viewModel;
        public ObservableCollection<string> Items { get; set; }

        public PlayerChampionshipSeriesPage(PlayerChampionshipSeriesViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //var tournament = e.Item as Result;
            //if (tournament == null)
            //    return;

            //await Navigation.PushAsync(new TournamentResultsPage(new TournamentResultsViewModel(tournament.TournamentId)));

            ////Deselect Item
            //((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.ChampionshipSeries.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }  
    }
}
