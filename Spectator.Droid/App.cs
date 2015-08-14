using System;
using Android.App;
using Android.Runtime;
using Gcm.Client;
using Microsoft.Practices.ServiceLocation;
using Spectator.Droid;
using Spectator.Core.Model;
using Spectator.Droid.Platform;
using Spectator.Droid.Platform.Gcm;

namespace Spectator.Droid
{
    [Application (Theme = "@style/AppTheme", HardwareAccelerated = true)]
	public class App : global::Android.App.Application
	{
		public static App Current { get; private set; }

		public App (IntPtr handle, JniHandleOwnership transfer) : base (handle, transfer)
		{
		}

		public override void OnCreate ()
		{
			base.OnCreate ();

			Current = this;
			InitDependencyInjections ();
			RegisterGcm ();
		}

		void InitDependencyInjections ()
		{
			var locator = new SpectatorServiceLocator (new AndroidInjectModule ());
			ServiceLocator.SetLocatorProvider (() => locator);
		}

		void RegisterGcm ()
		{
			#if DEBUG
			GcmClient.CheckManifest (this);
			#endif

			GcmClient.Register (this, GcmBroadcastReceiver.SENDER_IDS);
		}
	}
}