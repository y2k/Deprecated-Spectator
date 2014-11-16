// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace Spectator.Ios
{
	[Register ("CreateSubscriptionController")]
	partial class CreateSubscriptionController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField Link { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView RssItems { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField Title { get; set; }

		[Action ("HandleButtonOkPressed:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void HandleButtonOkPressed (UIButton sender);

		[Action ("HandlerButtonExtractRssPressed:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void HandlerButtonExtractRssPressed (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (Link != null) {
				Link.Dispose ();
				Link = null;
			}
			if (RssItems != null) {
				RssItems.Dispose ();
				RssItems = null;
			}
			if (Title != null) {
				Title.Dispose ();
				Title = null;
			}
		}
	}
}
