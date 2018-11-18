using Ifpa.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ifpa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        private int creatorIfpaNumber = 16927;

        AboutViewModel viewModel;

        public AboutPage()
        {
            InitializeComponent();

            BindingContext = this.viewModel = new AboutViewModel();
        }

        private async void CreatorLabel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlayerDetailPage(new PlayerDetailViewModel(creatorIfpaNumber)));
        }
    }
}