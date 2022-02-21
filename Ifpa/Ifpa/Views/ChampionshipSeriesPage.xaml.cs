﻿using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Nacs;
using PinballApi.Models.WPPR.v2.Series;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChampionshipSeriesPage : ContentPage
    {
        ChampionshipSeriesViewModel viewModel;
        int year = DateTime.Now.Year;

        public ChampionshipSeriesPage(string code)
        {
            InitializeComponent();

            //TODO: allow user to pick the year            

            BindingContext = this.viewModel = new ChampionshipSeriesViewModel(code, year);
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var championshipStandings = e.Item as SeriesOverallResult;
            if (championshipStandings == null)
                return;

            await Navigation.PushAsync(new ChampionshipSeriesDetailPage(new ChampionshipSeriesDetailViewModel(viewModel.SeriesCode, championshipStandings.RegionCode, year)));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.SeriesOverallResults.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            //caveat: NACS data started in 2018
            List<string> yearsToDisplay = new List<string>();

            for (int i = 2018; i <= DateTime.Now.Year; i++)
                yearsToDisplay.Add(i.ToString());

            string action = await DisplayActionSheet("Championship Series Year", "Cancel", null, yearsToDisplay.ToArray());

            if (int.TryParse(action, out var yearValue))
            {
                this.year = yearValue;
                viewModel.Year = yearValue;
                viewModel.LoadItemsCommand.Execute(null);
            }
        }
    }
}
