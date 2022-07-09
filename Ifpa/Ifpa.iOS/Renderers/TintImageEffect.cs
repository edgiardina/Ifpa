using Ifpa.iOS.Renderers;
using System;
using System.Linq;
using UIKit;
using Microsoft.Maui;
using Microsoft.Maui.Platform.iOS;
using FormsTintImageEffect = Ifpa.Effects.TintImageEffect;

[assembly: ResolutionGroupName(FormsTintImageEffect.GroupName)]
[assembly: ExportEffect(typeof(TintImageEffect), FormsTintImageEffect.Name)]
namespace Ifpa.iOS.Renderers
{
    public class TintImageEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var effect = (FormsTintImageEffect)Element.Effects.FirstOrDefault(e => e is FormsTintImageEffect);

                if (effect == null || !(Control is UIImageView image))
                    return;

                image.Image = image.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                image.TintColor = effect.TintColor.ToUIColor();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred when setting the {typeof(TintImageEffect)} effect: {ex.Message}\n{ex.StackTrace}");
            }
        }

        protected override void OnDetached() { }
    }
}