using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.Views;
using Xamarin.Essentials;
using Ifpa.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Ifpa
{
    public partial class App : Application
    {

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Constants.SyncFusionLicenseKey);

            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            VersionTracking.Track();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
