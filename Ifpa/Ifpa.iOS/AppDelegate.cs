using System;
using System.Linq;
using Foundation;
using Ifpa.iOS.Services;
using Ifpa.iOS.Utils;
using Ifpa.Models;
using Ifpa.Services;
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
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            Syncfusion.XForms.iOS.Expander.SfExpanderRenderer.Init();
            Syncfusion.SfCalendar.XForms.iOS.SfCalendarRenderer.Init();
            Syncfusion.SfPdfViewer.XForms.iOS.SfPdfDocumentViewRenderer.Init();

            // Get possible shortcut item
            if (options != null)
            {
                LaunchedShortcutItem = options[UIApplication.LaunchOptionsShortcutItemKey] as UIApplicationShortcutItem;
            }

            LoadApplication(new App());

            UIApplication.SharedApplication.RegisterUserNotificationSettings(UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Badge | UIUserNotificationType.Alert | UIUserNotificationType.Sound, null));
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            return base.FinishedLaunching(app, options);
        }

        public override async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            await NotificationService.NotifyIfUserHasNewlySubmittedTourneyResults();
            await NotificationService.NotifyIfUsersRankChanged();
            await NotificationService.NotifyIfNewBlogItemPosted();

            // Inform system of fetch results
            completionHandler(UIBackgroundFetchResult.NewData);
        }

        public override async void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            if (notification.AlertAction != BaseNotificationService.NewBlogPostTitle)
            {
                //Navigate to My Stats
                ((MainPage)(App.Current.MainPage)).CurrentPage = ((MainPage)(App.Current.MainPage)).Children.Single(n => n.Title == "My Stats");

                //Press Tournament Results Button.
                await (((MainPage)(App.Current.MainPage)).CurrentPage).Navigation.PushAsync(new ActivityFeedPage());
            }
            else
            {
                //Navigate to More
                ((MainPage)(App.Current.MainPage)).CurrentPage = ((MainPage)(App.Current.MainPage)).Children.Single(n => n.Title == "More");

                //Open News
                await (((MainPage)(App.Current.MainPage)).CurrentPage).Navigation.PushAsync(new NewsPage());
            }
        }

        #region QuickActions
        public UIApplicationShortcutItem LaunchedShortcutItem { get; set; }

        public bool HandleShortcutItem(UIApplicationShortcutItem shortcutItem)
        {
            var handled = false;

            // Anything to process?
            if (shortcutItem == null) return false;

            // Take action based on the shortcut type
            switch (shortcutItem.Type)
            {
                case ShortcutIdentifier.PlayerSearch:
                    Settings.CurrentTabIndex = 1;
                    handled = true;
                    break;
                case ShortcutIdentifier.MyStats:
                    Settings.CurrentTabIndex = 2;
                    handled = true;
                    break;
                case ShortcutIdentifier.Calendar:
                    Settings.CurrentTabIndex = 3;
                    handled = true;
                    break;
            }

             ((MainPage)(App.Current.MainPage)).SwitchTabToLastSelectedTab();

            // Return results
            return handled;
        }


        public override void OnActivated(UIApplication application)
        {
            base.OnActivated(application);

            // Handle any shortcut item being selected
            HandleShortcutItem(LaunchedShortcutItem);

            // Clear shortcut after it's been handled
            LaunchedShortcutItem = null;
        }

        public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
        {
            // Perform action
            completionHandler(HandleShortcutItem(shortcutItem));
        }

        #endregion
    }
}
