using Microsoft.Maui.Essentials;
using Ifpa.Models;
using Ifpa.Styles;
using System;
using Ifpa.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Web;
using Plugin.LocalNotification;

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

            NotificationCenter.Current.NotificationTapped += Current_NotificationTapped;

            MainPage = new AppShell();

            Current.RequestedThemeChanged += (s, a) =>
            {
                SetThemeBasedOnDeviceTheme();
            };
        }

        private async void Current_NotificationTapped(Plugin.LocalNotification.EventArgs.NotificationEventArgs e)
        {
            await Shell.Current.GoToAsync(e.Request.ReturningData);
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
            base.OnAppLinkRequestReceived(uri);

            //App Shortcuts
            if (uri.ToString().Contains(Shortcuts.AppShortcutUriBase))
            {
                var option = uri.ToString().Replace(Shortcuts.AppShortcutUriBase, "");
                if (!string.IsNullOrEmpty(option))
                {
                    if(option.Contains("ranking"))
                    {
                        //Note: iOS Xamarin appears broken in applinkrequests
                        //https://stackoverflow.com/questions/70719731/xamarin-forms-shell-gotoasync-is-not-working-as-expected-in-ios
                        Shell.Current.CurrentItem = Shell.Current.CurrentItem.Items[0];
                    }
                    await Shell.Current.GoToAsync($"//{option}");
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
                    Shell.Current.CurrentItem = Shell.Current.CurrentItem.Items[0];
                    await Shell.Current.GoToAsync($"//rankings/player-details?playerId={id}");
                }
            }
            //tournaments/view.php?t=46773
            else if (uri.ToString().Contains("tournaments/view.php")) 
            { 
                var id = HttpUtility.ParseQueryString(uri.Query)["t"];
                if (!string.IsNullOrEmpty(id))
                {
                    Shell.Current.CurrentItem = Shell.Current.CurrentItem.Items[0];
                    await Shell.Current.GoToAsync($"//rankings/tournament-results?tournamentId={id}");
                }
            }
        }
    }
}
