using Ifpa.ViewModels;
using PinballApi.Models.WPPR.v2.Nacs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChampionshipSeriesPage : ContentPage
    {
        ChampionshipSeriesViewModel viewModel;

        public ChampionshipSeriesPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new ChampionshipSeriesViewModel();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var championshipStandings = e.Item as NacsStandings;
            if (championshipStandings == null)
                return;

            await Navigation.PushAsync(new ChampionshipSeriesDetailPage(new ChampionshipSeriesDetailViewModel(championshipStandings.StateProvince)));

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
