using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using NotificationCenter;
using UIKit;

namespace Ifpa.iOS.RankExtension
{
    [Register("CodeBasedViewController")]
    public class CodeBasedViewController : UIViewController, INCWidgetProviding
    {
        public CodeBasedViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Add label to view
            var TodayMessage = new UILabel(new CGRect(0, 0, View.Frame.Width, View.Frame.Height))
            {
                TextAlignment = UITextAlignment.Center
            };

            View.AddSubview(TodayMessage);

            TodayMessage.Text = "Your rank is 206th";

        }
    }
}