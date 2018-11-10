using Ifpa.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        private int creatorIfpaNumber = 16927;

        public AboutPage()
        {
            InitializeComponent();
        }

        private async Task CreatorLabel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(creatorIfpaNumber)));
        }
    }
}