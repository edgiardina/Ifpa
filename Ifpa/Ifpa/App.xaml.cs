using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.Views;
using Xamarin.Essentials;
using Ifpa.Models;
using Ifpa.Styles;
using Ifpa.Interfaces;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Ifpa
{
    public partial class App : Application
    {               
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Constants.SyncFusionLicenseKey);
            //initial theme should be light 
            
            InitializeComponent();
            MainPage = new MainPage();
        }

        public static AppTheme AppTheme { get; set; }

        protected override void OnStart()
        {
            base.OnStart();
            // Handle when your app starts
            VersionTracking.Track();           

            SetThemeBasedOnDeviceTheme();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            base.OnResume();

            //BUG: For some reason this code never runs.
            // Handle when your app resumes
            AppTheme theme = DependencyService.Get<IThemeInspector>().GetOperatingSystemTheme();

            SetThemeBasedOnDeviceTheme();
        }

        public static void SetThemeBasedOnDeviceTheme()
        {
            AppTheme theme = DependencyService.Get<IThemeInspector>().GetOperatingSystemTheme();

            //Handle Light Theme & Dark Theme
            if (theme == AppTheme.Light)
                App.Current.Resources = new LightTheme();
            else
                App.Current.Resources = new DarkTheme();
        }
    }
}
