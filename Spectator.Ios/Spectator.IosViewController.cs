using System;
using System.Linq;
using MonoTouch.UIKit;
using FlyoutNavigation;
using MonoTouch.Dialog;
using System.Collections.Generic;
using Spectator.Core.Model.Database;
using Spectator.Core.Model;

namespace Spectator.Ios
{
	public partial class Spectator_IosViewController : UIViewController
	{
		IEnumerable<Subscription> subscriptions;

		public Spectator_IosViewController (IntPtr handle) : base (handle)
		{
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			var navigation = new FlyoutNavigationController {
				ViewControllers = new [] { new UIViewController () },
			};
			View.AddSubview (navigation.View);

			navigation.SelectedIndexChanged = () => {
				var currentController = (SnapshotListController)navigation.CurrentViewController;
				var sub = subscriptions.ElementAt (navigation.SelectedIndex);
				currentController.SetSubscriptionId (sub.Id);
			};

			LoadSubscriptions (navigation);
		}

		async void LoadSubscriptions (FlyoutNavigationController navigation)
		{
			var subsModel = new SubscriptionCollectionModel ();
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			await subsModel.Reload ();
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			subscriptions = await subsModel.Get ();

			navigation.NavigationRoot = new RootElement ("Navigation") {
				subscriptions.GroupBy (s => s.GroupTitle)
				.Select (s => {
					var sec = new Section (s.First ().GroupTitle);
					sec.AddAll (s.Select (i => new StringElement (i.Title)));
					return sec;
				})
			};
			navigation.ViewControllers = subscriptions
				.Select (s => (UIViewController)Storyboard.InstantiateViewController ("snapshots"))
				.ToArray ();
		}

		#endregion

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}
	}
}