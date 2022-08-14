using System;
using System.Linq;
using Foundation;
using Ifpa.Interfaces;
using Ifpa.iOS.Services;
using Ifpa.Services;
using Ifpa.Views;
using Microsoft.Extensions.DependencyInjection;
using UIKit;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;

namespace Ifpa.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : MauiUIApplicationDelegate
    {
        
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        
        NotificationService NotificationService = new NotificationService();
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //global::Xamarin.Forms.Forms.Init();
            //global::Xamarin.FormsMaps.Init();
            //Syncfusion.SfChart.XForms.iOS.Renderers.SfChartRenderer.Init();
            //Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            //Syncfusion.SfCalendar.XForms.iOS.SfCalendarRenderer.Init();
            //Syncfusion.SfPdfViewer.XForms.iOS.SfPdfDocumentViewRenderer.Init();

            // Ask the user for permission to show notifications on iOS 10.0+ at startup.
            // If not asked at startup, user will be asked when showing the first notification.
            //Plugin.LocalNotification.NotificationCenter.AskPermission();

            //LoadApplication(new App(PlatformSpecificServices));

            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            return base.FinishedLaunching(app, options);
        }

        public override async void WillEnterForeground(UIApplication uiApplication)
        {
            //await Plugin.LocalNotification.NotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
            base.WillEnterForeground(uiApplication);
        }

        

        //public override async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        //{
        //    try
        //    {
        //        await NotificationService.NotifyIfUserHasNewlySubmittedTourneyResults();
        //        await NotificationService.NotifyIfUsersRankChanged();
        //        await NotificationService.NotifyIfNewBlogItemPosted();

        //        // Inform system of fetch results
        //        completionHandler(UIBackgroundFetchResult.NewData);
        //    }
        //    catch
        //    {
        //        completionHandler(UIBackgroundFetchResult.Failed);
        //    }

        //}

        //static void PlatformSpecificServices(IServiceCollection services)
        //{
        //    services.AddSingleton<IReminderService, iOSReminderService>();
        //}

        #region QuickActions       

        //public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
        //{
        //    var uri = Plugin.AppShortcuts.iOS.ArgumentsHelper.GetUriFromApplicationShortcutItem(shortcutItem);
        //    if (uri != null)
        //    {
        //        Xamarin.Forms.Application.Current.SendOnAppLinkRequestReceived(uri);
        //    }
        //}

        #endregion
    }
}
