using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
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

        private async Task TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            //If a user hasn't set up my stats, redirect to player search

            var i = this.Children.IndexOf(this.CurrentPage);

            if (Preferences.Get("PlayerId", 0) == 0 && i == 2)
            {
                this.CurrentPage = this.Children[1];
                await DisplayAlert("Configure your Stats", "Looks like you haven't configured your 'My Stats' page. Use the Player Search to find your Player, and press the Star to configure your Stats", "OK");                
            }
        }
    }
}