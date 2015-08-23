// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Spectator.iOS
{
	[Register ("SnapshotViewController")]
	partial class SnapshotViewController
	{
		[Outlet]
		UIKit.UILabel CreatedLabel { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem DetailsButton { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem DiffButton { get; set; }

		[Outlet]
		UIKit.UIView InformationPanel { get; set; }

		[Outlet]
		UIKit.UICollectionView List { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel UrlLabel { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem WebButton { get; set; }

		[Outlet]
		UIKit.UIWebView WebView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CreatedLabel != null) {
				CreatedLabel.Dispose ();
				CreatedLabel = null;
			}

			if (DetailsButton != null) {
				DetailsButton.Dispose ();
				DetailsButton = null;
			}

			if (DiffButton != null) {
				DiffButton.Dispose ();
				DiffButton = null;
			}

			if (InformationPanel != null) {
				InformationPanel.Dispose ();
				InformationPanel = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (UrlLabel != null) {
				UrlLabel.Dispose ();
				UrlLabel = null;
			}

			if (WebButton != null) {
				WebButton.Dispose ();
				WebButton = null;
			}

			if (WebView != null) {
				WebView.Dispose ();
				WebView = null;
			}

			if (List != null) {
				List.Dispose ();
				List = null;
			}
		}
	}
}
