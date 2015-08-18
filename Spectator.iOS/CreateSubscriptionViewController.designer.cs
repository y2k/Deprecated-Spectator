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
	[Register ("CreateSubscriptionViewController")]
	partial class CreateSubscriptionViewController
	{
		[Outlet]
		UIKit.UIBarButtonItem CancelButton { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem DoneButton { get; set; }

		[Outlet]
		UIKit.UITextField Title { get; set; }

		[Outlet]
		UIKit.UITextField Url { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Title != null) {
				Title.Dispose ();
				Title = null;
			}

			if (Url != null) {
				Url.Dispose ();
				Url = null;
			}

			if (DoneButton != null) {
				DoneButton.Dispose ();
				DoneButton = null;
			}

			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}
		}
	}
}
