using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Ifpa.ViewModels;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        public CalendarViewModel viewModel { get; set; }

        public Location Location { get; set; }

        public Placemark Placemark { get; set; }

        public CalendarPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CalendarViewModel();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            DistanceText.Detail = ((int)DistanceSlider.Value).ToString();
            await viewModel.ExecuteLoadItemsCommand(ZipCodeEntry.Text, (int)DistanceSlider.Value);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            DistanceText.Detail = DistanceSlider.Value.ToString();

            try
            {
                Location = await Geolocation.GetLastKnownLocationAsync();

                var placemarks = await Geocoding.GetPlacemarksAsync(Location);

                Placemark = placemarks.First();
                ZipCodeEntry.Text = Placemark.PostalCode;
            }
            catch{ }

            if (viewModel.CalendarDetails.Count == 0)
            {
                await viewModel.ExecuteLoadItemsCommand(ZipCodeEntry.Text, (int)DistanceSlider.Value);
            }

        }
    }
}
