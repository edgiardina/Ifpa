using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Nacs;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChampionshipSeriesPage : ContentPage
    {
        ChampionshipSeriesViewModel viewModel;
        int year = DateTime.Now.Year;

        public ChampionshipSeriesPage()
        {
            InitializeComponent();

            //TODO: allow user to pick the year            

            BindingContext = this.viewModel = new ChampionshipSeriesViewModel(year);
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var championshipStandings = e.Item as NacsStandings;
            if (championshipStandings == null)
                return;

            await Navigation.PushAsync(new ChampionshipSeriesDetailPage(new ChampionshipSeriesDetailViewModel(championshipStandings.StateProvince, year)));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.StateProvinceStandings.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }  
    }
}
