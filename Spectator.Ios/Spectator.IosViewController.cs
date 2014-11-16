using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlyoutNavigation;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;

namespace Spectator.Ios
{
	public partial class Spectator_IosViewController : UIViewController
	{
		SubscriptionCollectionModel subsModel = new SubscriptionCollectionModel ();
		List<Subscription> subscriptions = new List<Subscription> ();

		public Spectator_IosViewController (IntPtr handle) : base (handle)
		{
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Add, (s, e) => {
				//
				PerformSegue ("createSubscription", this);
			});
			NavigationItem.Title = "Feed";

			// Perform any additional setup after loading the view, typically from a nib.
			var navigation = new FlyoutNavigationController {
				ViewControllers = new [] { new UIViewController () },
			};
			View.AddSubview (navigation.View);

			navigation.SelectedIndexChanged = () => {
				var currentController = (SnapshotListController)navigation.CurrentViewController;
				var sub = subscriptions.ElementAt (navigation.SelectedIndex);
				currentController.SetSubscriptionId (sub.Id);
				NavigationItem.Title = sub.Title;
			};

			LoadSubscriptions (navigation);
		}

		async void LoadSubscriptions (FlyoutNavigationController navigation)
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			await subsModel.Reload ();
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			await ReloadMenu ();

			navigation.NavigationRoot = new RootElement (null) {
				subscriptions
					.GroupBy (s => s.GroupTitle)
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

		async Task ReloadMenu ()
		{
			subscriptions.Clear ();
			subscriptions.Add (new Subscription { Id = 0, Title = "Feed" });
			subscriptions.AddRange (await subsModel.Get ());
		}

		#endregion

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}
	}
}