using System.Linq;
using Android.Graphics;
using Android.Widget;
using Ifpa.Droid.Renderers;
using Java.Lang;
using FormsTintImageEffect = Ifpa.Effects.TintImageEffect;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

[assembly: ResolutionGroupName(FormsTintImageEffect.GroupName)]
[assembly: ExportEffect(typeof(TintImageEffect), FormsTintImageEffect.Name)]
namespace Ifpa.Droid.Renderers
{
    public class TintImageEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var effect = (FormsTintImageEffect)Element.Effects.FirstOrDefault(e => e is FormsTintImageEffect);

                if (effect == null || !(Control is ImageView image))
                    return;

                var filter = new PorterDuffColorFilter(effect.TintColor.ToAndroid(), PorterDuff.Mode.SrcIn);
                image.SetColorFilter(filter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"An error occurred when setting the {typeof(TintImageEffect)} effect: {ex.Message}\n{ex.StackTrace}");
            }
        }

        protected override void OnDetached() { }
    }
}