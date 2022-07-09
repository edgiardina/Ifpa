using Ifpa.ViewModels;
using System;
using Microsoft.Maui;
using Microsoft.Maui.Xaml;
using Microsoft.Maui.Essentials;
using Ifpa.Models;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        private const int creatorIfpaNumber = 16927;
        private readonly AboutViewModel viewModel;

        public AboutPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new AboutViewModel();
        }

        private async void CreatorLabel_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"player-details?playerId={creatorIfpaNumber}");
        }

        private async void ReviewButton_Clicked(object sender, EventArgs e)
        {
            var url = string.Empty;

            if (Device.RuntimePlatform == Device.iOS)
            {                
                url = $"itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id={Constants.AppStoreAppId}&amp;onlyLatestVersion=true&amp;pageNumber=0&amp;sortOrdering=1&amp;type=Purple+Software";
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                url = $"https://play.google.com/store/apps/details?id={Constants.PlayStoreAppId}";
            }

            if (string.IsNullOrWhiteSpace(url))
                return;

            await Browser.OpenAsync(url, BrowserLaunchMode.External);
        }

        private async void LearnMore_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("http://tiltforums.com/t/ifpa-app-now-available-on-the-app-store/4543", BrowserLaunchMode.External);
        }

        private async void Flagpedia_Tapped(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://flagpedia.net/", BrowserLaunchMode.External);
        }
    }
}