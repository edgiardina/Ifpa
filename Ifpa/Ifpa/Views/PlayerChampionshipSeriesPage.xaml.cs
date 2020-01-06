using Ifpa.ViewModels;
using PinballApi.Models.v2.WPPR;
using System;
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
            var state = e.Item as ChampionshipSeries;
            if (state == null)
                return;

            await Navigation.PushAsync(new ChampionshipSeriesDetailPage(new ChampionshipSeriesDetailViewModel(state.StateProvince, state.Year)));

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
