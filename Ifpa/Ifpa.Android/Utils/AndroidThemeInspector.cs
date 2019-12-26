using System;
using System.Threading.Tasks;
using Android.Content.Res;
using Android.OS;
using Ifpa.Droid.Utils;
using Ifpa.Interfaces;
using Ifpa.Styles;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidThemeInspector))]
namespace Ifpa.Droid.Utils
{
    public class AndroidThemeInspector : IThemeInspector
    {

        public Task<AppTheme> GetOperatingSystemThemeAsync() =>
          Task.FromResult(GetOperatingSystemTheme());

        public AppTheme GetOperatingSystemTheme()
        {
            //Ensure the device is running Android Froyo or higher because UIMode was added in Android Froyo, API 8.0
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
            {
                var uiModelFlags = CrossCurrentActivity.Current.AppContext.Resources.Configuration.UiMode & UiMode.NightMask;

                switch (uiModelFlags)
                {
                    case UiMode.NightYes:
                        return AppTheme.Dark;

                    case UiMode.NightNo:
                        return AppTheme.Light;

                    default:
                        throw new NotSupportedException($"UiMode {uiModelFlags} not supported");
                }
            }
            else
            {
                return AppTheme.Light;
            }
        }
    }
}