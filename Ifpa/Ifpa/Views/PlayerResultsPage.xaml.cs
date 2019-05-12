using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2;
using PinballApi.Models.WPPR.v2.Players;
using System;
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
            var tournament = e.Item as PlayerResult;
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

        private void SfTabView_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            switch(e.Name)
            {
                case "Active":
                    viewModel.State = ResultType.Active;
                    break;
                case "Unused":
                    viewModel.State = ResultType.NonActive;
                    break;
                case "Past":
                    viewModel.State = ResultType.Inactive;
                    break;
            }          
            viewModel.LoadItemsCommand.Execute(null);
        }

        private void RankingProfileButton_Clicked(object sender, System.EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                viewModel.RankingType = (RankingType)Enum.Parse(typeof(RankingType), e.SelectedItem.ToString());           
                viewModel.LoadItemsCommand.Execute(null);
                navigationDrawer.IsOpen = false;
            }
        }
    }
}
