using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.Views;
using Xamarin.Essentials;
using Ifpa.Models;
using System.IO;
using Ifpa.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Ifpa
{
    public partial class App : Application
    {
        static ActivityFeedDatabase activityFeed;

        public static ActivityFeedDatabase ActivityFeed
        {
            get
            {
                if (activityFeed == null)
                {
                    activityFeed = new ActivityFeedDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ActivityFeedSQLite.db3"));
                }
                return activityFeed;
            }
        }
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
