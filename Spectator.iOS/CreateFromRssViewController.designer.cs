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
	[Register ("CreateFromRssViewController")]
	partial class CreateFromRssViewController
	{
		[Outlet]
		UIKit.UIActivityIndicatorView ActivitityIndicator { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem CancelButton { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem DoneButton { get; set; }

		[Outlet]
		UIKit.UIButton ExtractButton { get; set; }

		[Outlet]
		UIKit.UITextField LinkText { get; set; }

		[Outlet]
		UIKit.UINavigationBar NavigaionBar { get; set; }

		[Outlet]
		UIKit.UITableView RssList { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ActivitityIndicator != null) {
				ActivitityIndicator.Dispose ();
				ActivitityIndicator = null;
			}

			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}

			if (DoneButton != null) {
				DoneButton.Dispose ();
				DoneButton = null;
			}

			if (ExtractButton != null) {
				ExtractButton.Dispose ();
				ExtractButton = null;
			}

			if (LinkText != null) {
				LinkText.Dispose ();
				LinkText = null;
			}

			if (NavigaionBar != null) {
				NavigaionBar.Dispose ();
				NavigaionBar = null;
			}

			if (RssList != null) {
				RssList.Dispose ();
				RssList = null;
			}
		}
	}
}
