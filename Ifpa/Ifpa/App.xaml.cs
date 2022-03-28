using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ifpa.Views;
using Xamarin.Essentials;
using Ifpa.Models;
using Ifpa.Styles;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Ifpa
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Constants.SyncFusionLicenseKey);
            //initial theme should be light 

            _ = Shortcuts.AddShortcuts();

            InitializeComponent();
            MainPage = new AppShell();

            Current.RequestedThemeChanged += (s, a) =>
            {
                SetThemeBasedOnDeviceTheme();
            };
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
            base.OnSleep();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }    

        public static void SetThemeBasedOnDeviceTheme()
        {
            OSAppTheme currentTheme = Current.RequestedTheme;

            //Handle Light Theme & Dark Theme
            if (currentTheme == OSAppTheme.Dark)
            {
                Current.Resources.MergedDictionaries.Remove(new LightTheme());
                Current.Resources.MergedDictionaries.Add(new DarkTheme());
            }
            else
            {
                Current.Resources.MergedDictionaries.Remove(new DarkTheme());
                Current.Resources.MergedDictionaries.Add(new LightTheme());
            }
        }

        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            //App Shortcut
            if (uri.ToString().Contains(Shortcuts.AppShortcutUriBase))
            {
                var option = uri.ToString().Replace(Shortcuts.AppShortcutUriBase, "");
                if (!string.IsNullOrEmpty(option))
                {
                    await Shell.Current.GoToAsync($"///{option}");
                }
                else
                {
                    base.OnAppLinkRequestReceived(uri);
                }
            }

            //TODO: Deep Link request
        }
    }
}
