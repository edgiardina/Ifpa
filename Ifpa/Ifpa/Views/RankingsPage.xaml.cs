using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using Xamarin.Essentials;
using PinballApi.Models.WPPR.v2;
using System;
using PinballApi.Models.WPPR.v2.Rankings;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RankingsPage : ContentPage
    {
        RankingsViewModel viewModel;

        public RankingsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RankingsViewModel();
            PlayerListViewIndexConverter.BindingContext = viewModel;
            RankingTypePicker.SelectedIndex = 0;

            CountryPicker.IsVisible = false;
            CountryLabel.IsVisible = false;
            TypeLabel.IsVisible = false;
            TypePicker.IsVisible = false;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var player = args.SelectedItem as RankingResult;
            if (player == null)
                return;

            await Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(player.PlayerId)));

            // Manually deselect item.
            PlayersListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Players.Count == 0)
            {
                viewModel.CountOfItemsToFetch = Preferences.Get("PlayerCount", viewModel.CountOfItemsToFetch);
                viewModel.StartingPosition = Preferences.Get("StartingRank", viewModel.StartingPosition);
                viewModel.CurrentTournamentType = (TournamentType)Enum.Parse(typeof(TournamentType), Preferences.Get("TournamentType", viewModel.CurrentTournamentType.ToString()));
                viewModel.CountryToShow = viewModel.DefaultCountry;

                viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private void Picker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //don't fire selectedIndexChanged when we are adding countries first time in
            if (viewModel.Countries.Count > 0 && !viewModel.IsBusy)
            {
                Preferences.Set("CountryName", viewModel.CountryToShow.CountryName);
                viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (viewModel.Players.Count > 0)
            {
                Preferences.Set("PlayerCount", viewModel.CountOfItemsToFetch);
                Preferences.Set("StartingRank", viewModel.StartingPosition);
                viewModel.LoadItemsCommand.Execute(null);
            }            
        }

        private void RankingType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var selectedType = (RankingType)Enum.Parse(typeof(RankingType), ((Picker)sender).SelectedItem as string);

            if (selectedType == RankingType.Country)
            {
                CountryPicker.IsVisible = true;
                CountryLabel.IsVisible = true;
                TypeLabel.IsVisible = false;
                TypePicker.IsVisible = false;
            }
            else if(selectedType == RankingType.Women)
            {
                CountryPicker.IsVisible = false;
                CountryLabel.IsVisible = false;
                TypeLabel.IsVisible = true;
                TypePicker.IsVisible = true;
            }
            else
            {
                CountryPicker.IsVisible = false;
                CountryLabel.IsVisible = false;
                TypeLabel.IsVisible = false;
                TypePicker.IsVisible = false;
                viewModel.CountryToShow = viewModel.DefaultCountry;
            }

            viewModel.CurrentRankingType = selectedType;
            if (viewModel.Players.Count > 0)
            {
                viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!viewModel.IsBusy)
            {
                Preferences.Set("TournamentType", viewModel.CurrentTournamentType.ToString());
                viewModel.LoadItemsCommand.Execute(null);
            }
        }
    }
}