using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android;

namespace Ifpa.Droid
{
    [Activity(Label = "Ifpa", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.FormsMaps.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (ContextCompat.CheckSelfPermission(this, "ACCESS_FINE_LOCATION") != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation }, 0);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Permission to ACCESS_FINE_LOCATION Granted!!!");
            }
        }
    }
}