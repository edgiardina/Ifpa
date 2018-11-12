﻿using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Ifpa.ViewModels;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using PinballApi.Models.WPPR.Calendar;

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

        private async void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            DistanceText.Text = ((int)DistanceSlider.Value).ToString();
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
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    Location = await Geolocation.GetLastKnownLocationAsync();

                    var placemarks = await Geocoding.GetPlacemarksAsync(Location);

                    Placemark = placemarks.First();
                    LocationEntry.Text = Placemark.Locality + ", " + Placemark.AdminArea;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

                IsBusy = false;
            }
        }

        private async Task UpdateCalendarData()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    calendarMap.Pins.Clear();

                    var location = await Geocoding.GetLocationsAsync(LocationEntry.Text);
                    calendarMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.First().Latitude, location.First().Longitude), new Distance(DistanceSlider.Value * 1609.344)));

                    DistanceText.Text = ((int)DistanceSlider.Value).ToString();
                    await viewModel.ExecuteLoadItemsCommand(LocationEntry.Text, (int)DistanceSlider.Value);

                    foreach (var detail in viewModel.CalendarDetails)
                    {
                        var pin = new Pin();
                        var locations = await Geocoding.GetLocationsAsync(detail.Address1 + " " + detail.City + ", " + detail.State);
                        pin.Position = new Position(locations.First().Latitude, locations.First().Longitude);
                        pin.Label = detail.TournamentName;

                        //TODO: on pinpress scroll listview to find item. 
                        pin.Clicked += (sender, e) => 
                        {
                            TournamentListView.ScrollTo(detail, ScrollToPosition.MakeVisible, true);
                        };

                        calendarMap.Pins.Add(pin);
                    }
                }
                catch (Exception e)
                {
                    //don't let the calendar crash our entire app
                    Debug.WriteLine(e.Message);
                }

                IsBusy = false;
            }
        }

        private async Task TournamentListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var calendar = e.SelectedItem as CalendarDetails;
            if (calendar == null)
                return;

            await Navigation.PushAsync(new CalendarDetailPage(new CalendarDetailViewModel(calendar.CalendarId)));

            // Manually deselect item.
            TournamentListView.SelectedItem = null;
        }
    }
}