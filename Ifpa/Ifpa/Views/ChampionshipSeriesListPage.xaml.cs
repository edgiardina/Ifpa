using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Series;
using System.Linq;
using Microsoft.Maui;


namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChampionshipSeriesListPage : ContentPage
    {
        ChampionshipSeriesListViewModel viewModel;

        public ChampionshipSeriesListPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new ChampionshipSeriesListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.ChampionshipSeries.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void ChampionshipSeriesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() != null)
            {
                var championshipSeries = e.CurrentSelection.FirstOrDefault() as Series;

                await Shell.Current.GoToAsync($"champ-series?seriesCode={championshipSeries.Code}");

                //Deselect Item
                ((CollectionView)sender).SelectedItem = null;
            }
        }

    }
}