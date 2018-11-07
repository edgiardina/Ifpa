using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Ifpa.ViewModels;
using Xamarin.Forms.Maps;
using System.Diagnostics;

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
            await UpdateCalendarData();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
                        
            if (viewModel.CalendarDetails.Count == 0)
            {
                await PollAndUpdateUserLocation();
                await UpdateCalendarData();
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await UpdateCalendarData();
        }
        private async void MyLocation_Clicked(object sender, EventArgs e)
        {
            await PollAndUpdateUserLocation();
            await UpdateCalendarData();
        }

        private async Task PollAndUpdateUserLocation()
        {
            try
            {
                Location = await Geolocation.GetLastKnownLocationAsync();

                var placemarks = await Geocoding.GetPlacemarksAsync(Location);

                Placemark = placemarks.First();
                ZipCodeEntry.Text = Placemark.PostalCode;
            }
            catch { }
        }

        private async Task UpdateCalendarData()
        {
            try
            {
                calendarMap.Pins.Clear();

                var location = await Geocoding.GetLocationsAsync(ZipCodeEntry.Text);
                calendarMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.First().Latitude, location.First().Longitude), new Distance(DistanceSlider.Value * 1609.344)));

                DistanceText.Detail = ((int)DistanceSlider.Value).ToString();
                await viewModel.ExecuteLoadItemsCommand(ZipCodeEntry.Text, (int)DistanceSlider.Value);

                foreach(var detail in viewModel.CalendarDetails)
                {
                    var pin = new Pin();
                    var locations = await Geocoding.GetLocationsAsync(detail.Address1 + " " + detail.City + ", " + detail.State);
                    pin.Position = new Position(locations.First().Latitude, locations.First().Longitude);
                    pin.Label = detail.TournamentName;

                    calendarMap.Pins.Add(pin);
                }
            }
            catch(Exception e)
            {
                //don't let the calendar crash our entire app
                Debug.WriteLine(e.Message);
            }
        }
    }
}
