using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Nacs;
using PinballApi.Models.WPPR.v2.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;


namespace Ifpa.Views
{
    [QueryProperty("SeriesCode", "seriesCode")]
    [QueryProperty("Year", "year")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChampionshipSeriesPage : ContentPage
    {
        ChampionshipSeriesViewModel viewModel;
        public int Year { get; set; } = DateTime.Now.Year;
        public string SeriesCode { get; set; }

        public ChampionshipSeriesPage()
        {
            InitializeComponent();

            //TODO: allow user to pick the year            

            BindingContext = this.viewModel = App.GetViewModel<ChampionshipSeriesViewModel>(); ;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var championshipStandings = e.Item as SeriesOverallResult;
            if (championshipStandings == null)
                return;

            await Shell.Current.GoToAsync($"champ-series-detail?seriesCode={viewModel.SeriesCode}&regionCode={championshipStandings.RegionCode}&year={Year}");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.SeriesOverallResults.Count == 0)
            {
                viewModel.Year = Year;
                viewModel.SeriesCode = SeriesCode;
                viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Championship Series Year", "Cancel", null, viewModel.AvailableYears.Select(n => n.ToString()).ToArray());

            if (int.TryParse(action, out var yearValue))
            {
                this.Year = yearValue;
                viewModel.Year = yearValue;
                viewModel.LoadItemsCommand.Execute(null);
            }
        }
    }
}
