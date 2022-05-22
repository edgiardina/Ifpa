using Xamarin.Forms;
using Xamarin.Essentials;
using Ifpa.Models;
using Ifpa.Styles;
using System;
using Ifpa.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Web;

namespace Ifpa
{
    public partial class App : Application
    {
        protected static IServiceProvider ServiceProvider { get; set; }

        public App(Action<IServiceCollection> addPlatformServices = null)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Constants.SyncFusionLicenseKey);
            //initial theme should be light 

            //DI and other service prep
            SetupServices(addPlatformServices);

            _ = Shortcuts.AddShortcuts();

            InitializeComponent();
            MainPage = new AppShell();

            Current.RequestedThemeChanged += (s, a) =>
            {
                SetThemeBasedOnDeviceTheme();
            };
        }

        private void SetupServices(Action<IServiceCollection> addPlatformServices)
        {
            var services = new ServiceCollection();

            //TODO: Add platform specific services and eliminate DependencyService
            addPlatformServices?.Invoke(services);

            //This extension method call will register all viewmodels in the same namespace as the BaseViewModel
            services.AddViewModels<BaseViewModel>();

            ServiceProvider = services.BuildServiceProvider();
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

        public static TViewModel GetViewModel<TViewModel>()
            where TViewModel : BaseViewModel
        {
            return ServiceProvider.GetService<TViewModel>();
        }

        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            //App Shortcuts
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

                return;
            }
            
            //DeepLinks
            if (uri.ToString().Contains("player.php"))
            {
                //extract player ID from querystring
                var id = HttpUtility.ParseQueryString(uri.Query)["p"];

                if (!string.IsNullOrEmpty(id))
                {
                    await Shell.Current.GoToAsync($"///rankings/player-details?playerId={id}");
                }
            }
            //tournaments/view.php?t=46773
            else if (uri.ToString().Contains("tournaments/view.php")) 
            { 
                var id = HttpUtility.ParseQueryString(uri.Query)["t"];
                if (!string.IsNullOrEmpty(id))
                {
                    await Shell.Current.GoToAsync($"///rankings/tournament-results?tournamentId={id}");
                }
            }
        }
    }
}
