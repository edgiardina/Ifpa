// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Ifpa.iOS.Today
{
    [Register ("TodayViewController")]
    partial class TodayViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView PlayerImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PlayerName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PlayerRank { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (PlayerImage != null) {
                PlayerImage.Dispose ();
                PlayerImage = null;
            }

            if (PlayerName != null) {
                PlayerName.Dispose ();
                PlayerName = null;
            }

            if (PlayerRank != null) {
                PlayerRank.Dispose ();
                PlayerRank = null;
            }
        }
    }
}