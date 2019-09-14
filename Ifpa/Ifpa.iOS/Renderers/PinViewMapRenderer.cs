using Ifpa.iOS.Renderers;
using Ifpa.Views.Controls;
using MapKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PinViewMap), typeof(PinViewMapRenderer))]
namespace Ifpa.iOS.Renderers
{
    public class PinViewMapRenderer : MapRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var map = Control as MKMapView;
                map.DidDeselectAnnotationView += (object sender, MKAnnotationViewEventArgs eventArgs) =>
                {
                    foreach (var anno in ((MKMapView)sender).Annotations)
                    {                       
                        ((MKMapView)sender).SelectAnnotation(anno, true);
                    }
                };
                map.DidFinishRenderingMap += (object sender, MKDidFinishRenderingMapEventArgs eventArgs) =>
                {
                    foreach (var anno in ((MKMapView)sender).Annotations)
                    {
                        ((MKMapView)sender).SelectAnnotation(anno, true);
                    }
                };

            }
        }
    }
}
