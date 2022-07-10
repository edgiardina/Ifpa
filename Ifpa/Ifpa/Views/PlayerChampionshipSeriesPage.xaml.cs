using Ifpa.ViewModels;
using PinballApi.Models.v2.WPPR;
using System;
using System.Collections.ObjectModel;
using Microsoft.Maui;


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
            var state = e.Item as ChampionshipSeries;
            if (state == null)
                return;

            await Shell.Current.GoToAsync($"champ-series-detail?seriesCode={state.SeriesCode}&regionCode={state.RegionCode}&year={state.Year}");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.ChampionshipSeries.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }  
    }
}
