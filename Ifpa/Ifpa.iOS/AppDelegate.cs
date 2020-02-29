using System;
using System.Linq;
using Foundation;
using Ifpa.iOS.Services;
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

            LoadApplication(new App());

            UIApplication.SharedApplication.RegisterUserNotificationSettings(UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Badge | UIUserNotificationType.Alert | UIUserNotificationType.Sound, null));
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            return base.FinishedLaunching(app, options);
        }

        public override async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            try
            {
                await NotificationService.NotifyIfUserHasNewlySubmittedTourneyResults();
                await NotificationService.NotifyIfUsersRankChanged();
                await NotificationService.NotifyIfNewBlogItemPosted();

                // Inform system of fetch results
                completionHandler(UIBackgroundFetchResult.NewData);
            }
            catch
            {
                completionHandler(UIBackgroundFetchResult.Failed);
            }

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

        public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
        {
            var uri = Plugin.AppShortcuts.iOS.ArgumentsHelper.GetUriFromApplicationShortcutItem(shortcutItem);
            if (uri != null)
            {
                Xamarin.Forms.Application.Current.SendOnAppLinkRequestReceived(uri);
            }
        }

        #endregion
    }
}
