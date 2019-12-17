using Ifpa.Styles;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(Ifpa.iOS.Renderers.PageRenderer))]
namespace Ifpa.iOS.Renderers
{
    public class PageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                SetAppTheme();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"\t\t\tERROR: {ex.Message}");
            }
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);
            Console.WriteLine($"TraitCollectionDidChange: {TraitCollection.UserInterfaceStyle} != {previousTraitCollection.UserInterfaceStyle}");

            if (this.TraitCollection.UserInterfaceStyle != previousTraitCollection.UserInterfaceStyle)
            {
                SetAppTheme();
            }
        }

        void SetAppTheme()
        {
            if (this.TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
            {
                if (App.AppTheme == AppTheme.Dark)
                    return;

                App.Current.Resources = new DarkTheme();

                App.AppTheme = AppTheme.Dark;
            }
            else
            {
                if (App.AppTheme == AppTheme.Light && App.Current.Resources.Count > 0)
                    return;

                App.Current.Resources = new LightTheme();
                App.AppTheme = AppTheme.Light;
            }
        }
    }
}