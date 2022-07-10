using Ifpa.iOS.Services;
using Ifpa.iOS.Utils;
using Ifpa.Services;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

[assembly: Dependency(typeof(ToolbarItemBadgeService))]
namespace Ifpa.iOS.Services
{
    public class ToolbarItemBadgeService : IToolbarItemBadgeService
    {
        public void SetBadge(Page page, ToolbarItem item, string value, Color backgroundColor, Color textColor)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var renderer = Platform.GetRenderer(page);
                if (renderer == null)
                {
                    renderer = Platform.CreateRenderer(page);
                    Platform.SetRenderer(page, renderer);
                }
                var vc = renderer.ViewController;

                var rightButtomItems = vc?.ParentViewController?.NavigationItem?.RightBarButtonItems;
                var idx = page.ToolbarItems.IndexOf(item);
                var reverseItemIndex = rightButtomItems.GetUpperBound(0) - idx;
                if (rightButtomItems != null && rightButtomItems.Length > idx)
                {
                    var barItem = rightButtomItems[reverseItemIndex];
                    if (barItem != null)
                    {
                        barItem.UpdateBadge(value, backgroundColor.ToUIColor(), textColor.ToUIColor());
                    }
                }
            });
        }
    }
}