using System;
using Microsoft.Practices.ServiceLocation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Spectator.Core.Model.Inject;
using Spectator.Core.Model.Push;
using Spectator.Ios.Model;

namespace Spectator.Ios
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window {
			get;
			set;
		}

		public override void FinishedLaunching (UIApplication application)
		{
			var locator = new SpectatorServiceLocator (new PlatformInjectModule ());
			ServiceLocator.SetLocatorProvider (() => locator);

			UIApplication.SharedApplication.RegisterForRemoteNotifications ();
		}

		public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		{
			new PushModel ().HandleNewUserToken (deviceToken.ToString ());
		}

		public override async void DidReceiveRemoteNotification (UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			await new PushModel ().HandleNewSyncMessage ();
			completionHandler (UIBackgroundFetchResult.NewData);
		}
	}
}