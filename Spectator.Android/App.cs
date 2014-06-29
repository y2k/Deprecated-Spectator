using System;
using Android.App;
using Android.Runtime;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Inject;

namespace Spectator.Android
{
	[Application]
	public class App : Application
	{
		public static Application Instance { get; private set; }

		public App(IntPtr handle, JniHandleOwnership transfer) : base(handle,transfer) { }

		public override void OnCreate ()
		{
			base.OnCreate ();

			Instance = this;
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator());
		}
	}
}