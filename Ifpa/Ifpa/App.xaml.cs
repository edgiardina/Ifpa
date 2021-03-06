﻿using Xamarin.Forms;
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
            MainPage = new MainPage();

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

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            var option = uri.ToString().Replace(Shortcuts.AppShortcutUriBase, "");
            if (!string.IsNullOrEmpty(option))
            {
                switch (option)
                {
                    case "playersearch":
                        Settings.CurrentTabIndex = 1;                
                        break;
                    case "mystats":
                        Settings.CurrentTabIndex = 2;           
                        break;
                    case "calendar":
                        Settings.CurrentTabIndex = 3;            
                        break;
                }

                ((MainPage)(Current.MainPage)).SwitchTabToLastSelectedTab();
            }
            else
            {
                base.OnAppLinkRequestReceived(uri);
            }
        }
    }
}
