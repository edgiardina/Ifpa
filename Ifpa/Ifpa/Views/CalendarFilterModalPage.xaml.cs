using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarFilterModalPage : ContentPage
    {
        public delegate void FilterSavedDelegate();

        public event FilterSavedDelegate FilterSaved;

        public CalendarFilterModalPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var lastCalendarLocation = Preferences.Get("LastCalendarLocation", "Unset");
            var lastCalendarDistance = Preferences.Get("LastCalendarDistance", 0);
            if (lastCalendarLocation == "Unset" || lastCalendarDistance == 0)
            {
                await PollAndUpdateUserLocation();
            }
            else
            {
                DistanceSlider.Value = lastCalendarDistance;
                LocationEntry.Text = lastCalendarLocation;
                DistanceText.Text = ((int)DistanceSlider.Value).ToString();
            }
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            DistanceText.Text = ((int)DistanceSlider.Value).ToString();
        }

        private async Task PollAndUpdateUserLocation()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    var location = await Geolocation.GetLastKnownLocationAsync();

                    var placemarks = await Geocoding.GetPlacemarksAsync(location);

                    var placemark = placemarks.First();
                    LocationEntry.Text = placemark.Locality + ", " + placemark.AdminArea;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

                IsBusy = false;
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await PollAndUpdateUserLocation();
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void FindButton_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("LastCalendarLocation", LocationEntry.Text);
            Preferences.Set("LastCalendarDistance", (int)DistanceSlider.Value);

            await Navigation.PopModalAsync();
            FilterSaved?.Invoke();
        }
    }
}