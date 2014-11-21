using System;
using Android.App;
using Android.Runtime;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Inject;
using Spectator.Android.Application.Model;
using Gcm.Client;
using Spectator.Android.Application.Model.Gcm;

namespace Spectator.Android.Application
{
	[Application]
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
			var locator = new SpectatorServiceLocator (new AndroidInjectModule ());
			ServiceLocator.SetLocatorProvider (() => locator);

			RegisterGcm ();
		}

		void RegisterGcm ()
		{
			#if DEBUG
			GcmClient.CheckDevice (this);
			GcmClient.CheckManifest (this);
			#endif

			GcmClient.Register (this, GcmBroadcastReceiver.SENDER_IDS);
		}
	}
}