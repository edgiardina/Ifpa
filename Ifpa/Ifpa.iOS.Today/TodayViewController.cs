using System;
using NotificationCenter;
using Foundation;
using Social;
using UIKit;
using PinballApi.Models.WPPR.v2.Players;
using PinballApi;
using PinballApi.Extensions;
using Ifpa.Models;
using System.Diagnostics;

namespace Ifpa.iOS.Today
{
    public partial class TodayViewController : SLComposeServiceViewController, INCWidgetProviding
    {
        public Player PlayerRecord { get; set; }
        public PinballRankingApiV2 PinballRankingApiV2 => new PinballRankingApiV2(Constants.IfpaApiKey);

        public NSUserDefaults plist = new NSUserDefaults(Constants.iOSAppGroup, NSUserDefaultsType.SuiteName);

        public TodayViewController(IntPtr handle) : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            Debug.WriteLine(plist.IntForKey("PlayerId"));

            var playerId = Convert.ToInt32(plist.IntForKey("PlayerId"));

            if (playerId > 0)
            {
                PlayerRecord = await PinballRankingApiV2.GetPlayer(playerId);

                PlayerImage.Image = FromUrl(PlayerRecord.ProfilePhoto.ToString());
                PlayerName.Text = PlayerRecord.FirstName + " " + PlayerRecord.LastName;
                PlayerRank.Text = PlayerRecord.PlayerStats.CurrentWpprRank.OrdinalSuffix();
                PlayerNotAssignedLabel.Hidden = true;
                PlayerInfoView.Hidden = false;
            }
            else
            {
                PlayerInfoView.Hidden = true; 
                PlayerNotAssignedLabel.Hidden = false;
            }
        }

        static UIImage FromUrl(string uri)
        {
            using (var url = new NSUrl(uri))
            using (var data = NSData.FromUrl(url))
                return UIImage.LoadFromData(data);
        }

        [Export("widgetPerformUpdateWithCompletionHandler:")]
        public void WidgetPerformUpdate(Action<NCUpdateResult> completionHandler)
        {
            // Perform any setup necessary in order to update the view.

            // If an error is encoutered, use NCUpdateResultFailed
            // If there's no update required, use NCUpdateResultNoData
            // If there's an update, use NCUpdateResultNewData

            // Do any additional setup after loading the view.
            completionHandler(NCUpdateResult.NewData);
        }
    }
}
