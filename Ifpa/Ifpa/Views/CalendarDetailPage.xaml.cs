using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarDetailPage : ContentPage
    {
        CalendarDetailViewModel viewModel;
        
        public CalendarDetailPage(CalendarDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;            
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();          

            viewModel.LoadItemsCommand.Execute(null);
        }

        private async Task WebsiteLabel_Tapped(object sender, EventArgs e)
        {
            await Browser.OpenAsync(viewModel.Website, BrowserLaunchMode.SystemPreferred);
        }

        private async Task Address_Tapped(object sender, EventArgs e)
        {
            var placemark = new Placemark
            {
                CountryName = viewModel.CountryName,
                AdminArea = viewModel.State,
                Thoroughfare = viewModel.Address1,
                Locality = viewModel.City
            };
            var options = new MapsLaunchOptions { Name = viewModel.TournamentName };

            await Maps.OpenAsync(placemark, options);
        }
    }
}