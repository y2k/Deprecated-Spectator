using System;
using MonoTouch.UIKit;
using Spectator.Core.Model;
using System.Drawing;

namespace Spectator.Ios
{
	partial class CreateSubscriptionController : UIViewController
	{
		UIAlertView progressDialog = new UIAlertView ("Creating subscription", "Please wait", null, null);

		public CreateSubscriptionController (IntPtr handle) : base (handle)
		{
		}

		partial void HandlerButtonExtractRssPressed (UIButton sender)
		{
			if (ValidateInputForExtractRss ())
				ExtractRss ();
		}

		bool ValidateInputForExtractRss ()
		{
			if (!Uri.IsWellFormedUriString (Link.Text, UriKind.Absolute)) {
				ShowErrorDialog ("Not valid URL for page with RSS");
				return false;
			}
			return true;
		}

		async void ExtractRss ()
		{
			SetIsBusy (true);
			try {
				var rss = await new RssExtractor (new Uri (Link.Text)).ExtracRss ();
				ChangeRssItems (rss);
			} catch {
				ShowErrorDialog ("Can't extract RSS from page");
			} finally {
				SetIsBusy (false);
			}
		}

		void ChangeRssItems (RssExtractor.RssItem[] rss)
		{
			foreach (var view in RssItems.Subviews)
				view.RemoveFromSuperview ();
			float y = 0;
			foreach (var item in rss) {
				var button = new UIButton (UIButtonType.System);
				button.SetTitle (item.Title, UIControlState.Normal);
				button.AddTarget ((s, e) => {
					Title.Text = item.Title;
					Link.Text = "" + item.Link;
				}, UIControlEvent.TouchUpInside);
				button.Frame = new RectangleF (0, y, View.Frame.Width, 40);
				y += 40;
				RssItems.AddSubview (button);
			}
		}

		partial void HandleButtonOkPressed (UIButton sender)
		{
			if (ValidateInputForCreateSubscription ())
				CreateSubscription ();
		}

		bool ValidateInputForCreateSubscription ()
		{
			if (string.IsNullOrEmpty (Title.Text)) {
				ShowErrorDialog ("Title can't be empty");
				return false;
			}
			if (!Uri.IsWellFormedUriString (Link.Text, UriKind.Absolute)) {
				ShowErrorDialog ("Not valid URL");
				return false;
			}
			return true;
		}

		void ShowErrorDialog (string message)
		{
			new UIAlertView ("Error", message, null, "OK").Show ();
		}

		async void CreateSubscription ()
		{
			try {
				SetIsBusy (true);
				await new SubscriptionModel ().CreateNew (new Uri (Link.Text), Title.Text);
				NavigationController.PopViewControllerAnimated (true);
			} catch {
				ShowErrorDialog ("Can't create subscription");
			} finally {
				SetIsBusy (false);
			}
		}

		void SetIsBusy (bool isBusy)
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = isBusy;
			if (isBusy)
				progressDialog.Show ();
			else
				progressDialog.DismissWithClickedButtonIndex (0, true);
		}
	}
}