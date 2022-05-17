using Ifpa.Droid.Renderers;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using Ifpa.Views.Controls;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
[assembly: ExportRenderer(typeof(PinViewMap), typeof(CustomMapRenderer))]
namespace Ifpa.Droid.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        public CustomMapRenderer(Context context) : base(context)
        {

        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (l + t + r + b != 0) //Passing a 0-sum layout to base.OnLayout would sometimes cause VS to disconnect from code. Adding this for my sanity.
            {
                base.OnLayout(changed, l, t, r, b);

                //Handle the theme.
                if (AppInfo.RequestedTheme == AppTheme.Dark)
                {
                    NativeMap?.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this.Context, Resource.Raw.MapsDarkMode));
                }
                else
                {
                    NativeMap?.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this.Context, Resource.Raw.MapsLightMode));
                }
            }
        }
    }
}