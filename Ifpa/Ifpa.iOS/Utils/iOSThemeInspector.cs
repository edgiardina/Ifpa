using System;
using System.Linq;
using System.Threading.Tasks;
using Ifpa.Interfaces;
using Ifpa.iOS.Utils;
using Ifpa.Styles;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSThemeInspector))]
namespace Ifpa.iOS.Utils
{
    public class iOSThemeInspector : IThemeInspector
    {
        public AppTheme GetOperatingSystemTheme()
        {
            //Ensure the current device is running 12.0 or higher, because `TraitCollection.UserInterfaceStyle` was introduced in iOS 12.0
            if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
            {
                var currentUIViewController = GetVisibleViewController();

                var userInterfaceStyle = currentUIViewController.TraitCollection.UserInterfaceStyle;

                switch (userInterfaceStyle)
                {
                    case UIUserInterfaceStyle.Light:
                        return AppTheme.Light;
                    case UIUserInterfaceStyle.Dark:
                        return AppTheme.Dark;
                    default:
                        throw new NotSupportedException($"UIUserInterfaceStyle {userInterfaceStyle} not supported");
                }
            }
            else
            {
                return AppTheme.Light;
            }
        }

        // UIApplication.SharedApplication can only be referenced by the Main Thread, so we'll use Device.InvokeOnMainThreadAsync which was introduced in Xamarin.Forms v4.2.0
        public async Task<AppTheme> GetOperatingSystemThemeAsync() => await Device.InvokeOnMainThreadAsync(GetOperatingSystemTheme);

        static UIViewController GetVisibleViewController()
        {
            UIViewController viewController = null;

            var window = UIApplication.SharedApplication.KeyWindow;

            if (window.WindowLevel == UIWindowLevel.Normal)
                viewController = window.RootViewController;

            if (viewController is null)
            {
                window = UIApplication.SharedApplication
                    .Windows
                    .OrderByDescending(w => w.WindowLevel)
                    .FirstOrDefault(w => w.RootViewController != null && w.WindowLevel == UIWindowLevel.Normal);

                viewController = window?.RootViewController ?? throw new InvalidOperationException("Could not find current view controller.");
            }

            while (viewController.PresentedViewController != null)
                viewController = viewController.PresentedViewController;

            return viewController;
        }
    }
}
