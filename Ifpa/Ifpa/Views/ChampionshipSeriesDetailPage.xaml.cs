using Ifpa.ViewModels;
using PinballApi.Models.v2.WPPR;
using PinballApi.Models.WPPR.v1.Players;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChampionshipSeriesDetailPage : ContentPage
    {
        ChampionshipSeriesDetailViewModel viewModel;

        public ChampionshipSeriesDetailPage(ChampionshipSeriesDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //var state = e.Item as ChampionshipSeries;
            //if (state == null)
            //    return;

            //await Navigation.PushAsync(new ChampionshipSeriesDetailPage(new ChampionshipSeriesDetailViewModel(state.GroupCode)));

            ////Deselect Item
            //((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.StateProvinceStandings.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }  
    }
}
