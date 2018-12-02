﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Linq;
using Ifpa.Interfaces;

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
            ((CalendarDetailViewModel)BindingContext).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(async (s, e) => await CalendarDetailPage_PropertyChanged(s, e)) ;
        }

        private async Task CalendarDetailPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //when busy is flipped back to false it means we are done loading the address
            if (e.PropertyName == "IsBusy" && viewModel.IsBusy == false)
            {
                var locations =  await Geocoding.GetLocationsAsync(viewModel.Address1 + " " + viewModel.City + ", " + viewModel.State);                                                        

                var position = new Position(locations.First().Latitude, locations.First().Longitude);
                calendarMap.MoveToRegion(new MapSpan(position, 0.1, 0.1));
                calendarMap.Pins.Add(new Pin
                {
                    Label = viewModel.TournamentName,
                    Position = position
                });
                calendarMap.IsVisible = true;
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();          

            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void WebsiteLabel_Tapped(object sender, EventArgs e)
        {
            await Browser.OpenAsync(viewModel.Website, BrowserLaunchMode.SystemPreferred);
        }

        private async void Address_Tapped(object sender, EventArgs e)
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

        private async void AddToCalendarButton_Clicked(object sender, EventArgs e)
        {
            var result = await DependencyService.Get<IReminderService>().CreateReminder(this.viewModel);

            if(result)
            {
                await DisplayAlert("Success", "Tournament added to your Calendar", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Unable to add Tournament to your Calendar", "OK");
            }
        }
    }
}