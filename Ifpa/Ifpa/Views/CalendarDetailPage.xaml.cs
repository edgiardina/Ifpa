using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Linq;
using Ifpa.Interfaces;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

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

            viewModel.LoadItemsCommand.Execute(null);
        }

        private async Task CalendarDetailPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //when busy is flipped back to false it means we are done loading the address
            if (e.PropertyName == "IsBusy" && viewModel.IsBusy == false)
            {
                try
                {
                    var locations = await Geocoding.GetLocationsAsync(viewModel.Address1 + " " + viewModel.City + ", " + viewModel.State);

                    var position = new Position(locations.First().Latitude, locations.First().Longitude);
                    calendarMap.MoveToRegion(new MapSpan(position, 0.1, 0.1));
                    var pin = new Pin
                    {
                        Label = viewModel.TournamentName,
                        Position = position,
                        Address = viewModel.Location,
                        Type = PinType.Generic                        
                    };

                    pin.InfoWindowClicked += Pin_Clicked;
               
                    calendarMap.Pins.Add(pin);
                    
                    calendarMap.IsVisible = true;
                }
                //unable to geocode position on the map, ignore. 
                catch(Exception)
                {
                    calendarMap.IsVisible = false;
                }                
            }
        }

        private void Pin_Clicked(object sender, EventArgs e)
        {
            Address_Tapped(sender, e);
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
            var options = new MapLaunchOptions { Name = viewModel.TournamentName };

            await Xamarin.Essentials.Map.OpenAsync(placemark, options);
        }

        private async void AddToCalendarButton_Clicked(object sender, EventArgs e)
        {
            var status = await Permissions.CheckStatusAsync<Permissions.CalendarWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.CalendarWrite>();
            }

            if (status == PermissionStatus.Granted)
            {
                string selectedCalendar = null;
                var calendars = await DependencyService.Get<IReminderService>().GetCalendarList();

                //iOS Supports multiple calendars. no idea how to do this in Android yet. 
                if (Device.RuntimePlatform == Device.iOS)
                {
                    selectedCalendar = await DisplayActionSheet("This event will be added to your phone's calendar", "Cancel", null, calendars.ToArray());
                }

                if (selectedCalendar != "Cancel")
                {
                    var result = await DependencyService.Get<IReminderService>().CreateReminder(this.viewModel, selectedCalendar);

                    if (result)
                    {
                        await DisplayAlert("Success", "Tournament added to your Calendar", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Unable to add Tournament to your Calendar", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Permission Required", "IFPA Companion requires your permission before adding items to your Calendar", "OK");
            }
        }


        private async void ShareButton_Clicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = $"https://www.ifpapinball.com/tournaments/view.php?t={viewModel.TournamentId}",
                Title = "Share Upcoming Tournament"
            });
        }
    }
}