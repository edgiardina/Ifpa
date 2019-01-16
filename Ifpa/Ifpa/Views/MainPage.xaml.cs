using Ifpa.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Application.Current.Properties.ContainsKey("tab_state"))
            {
                CurrentPage = Children[Int32.Parse(Application.Current.Properties["tab_state"].ToString())];
            }
        }
        
        protected override void OnCurrentPageChanged()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var i = this.Children.IndexOf(this.CurrentPage);

                Application.Current.Properties["tab_state"] = i;

                await Application.Current.SavePropertiesAsync();

                //If a user hasn't set up my stats, redirect to player search
                if (!Settings.HasConfiguredMyStats && i == 2)
                {
                    this.CurrentPage = this.Children[1];
                    await DisplayAlert("Configure your Stats", "Looks like you haven't configured your 'My Stats' page. Use the Player Search to find your Player, and press the Star to configure your Stats", "OK");
                    await this.CurrentPage.Navigation.PopToRootAsync();
                }
            });

            base.OnCurrentPageChanged();
        }

    }
}