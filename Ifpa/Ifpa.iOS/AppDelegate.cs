﻿using System;
using Foundation;
using Ifpa.iOS.Services;
using Ifpa.ViewModels;
using Ifpa.Views;
using UIKit;

namespace Ifpa.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        iOSNotificationService NotificationService = new iOSNotificationService();
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.FormsMaps.Init();
            Syncfusion.SfChart.XForms.iOS.Renderers.SfChartRenderer.Init();
            LoadApplication(new App());

            UIApplication.SharedApplication.RegisterUserNotificationSettings(UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Badge | UIUserNotificationType.Alert | UIUserNotificationType.Sound, null));

            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            return base.FinishedLaunching(app, options);
        }

        public override async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            await NotificationService.NotifyIfUserHasNewlySubmittedTourneyResults();
            await NotificationService.NotifyIfUsersRankChanged();

            // Inform system of fetch results
            completionHandler(UIBackgroundFetchResult.NewData);
        }

        public override async void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            if (notification.AlertAction == NotificationService.NewTournamentNotificationTitle)
            {
                //Navigate to My Stats
                ((MainPage)(App.Current.MainPage)).CurrentPage = ((MainPage)(App.Current.MainPage)).Children[2];

                var plist = NSUserDefaults.StandardUserDefaults;
                var playerId = Convert.ToInt32(plist.IntForKey("PlayerId"));

                //Press Tournament Results Button.
                await (((MainPage)(App.Current.MainPage)).CurrentPage).Navigation.PushAsync(new PlayerResultsPage(new PlayerResultsViewModel(playerId)));
            }
            else if(notification.AlertAction == NotificationService.NewRankNotificationTitle)
            {
                //Navigate to My Stats
                ((MainPage)(App.Current.MainPage)).CurrentPage = ((MainPage)(App.Current.MainPage)).Children[2];           
            }

            // reset our badge
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }
    }
}
