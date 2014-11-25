using System;
using Android.Support.V4.App;

namespace Spectator.Android.Application.Activity.Common.Base
{
	public class BaseFragment : Fragment
	{
		Action<EventHandler> onStart;
		Action<EventHandler> onStop;
		EventHandler handler;

		protected void AddStartStopEvent (EventHandler handler, Action<EventHandler> onStart, Action<EventHandler> onStop)
		{
			this.onStart = onStart;
			this.onStop = onStop;
			this.handler = handler;
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			onStart = onStop = null;
		}

		public override void OnStop ()
		{
			base.OnStop ();
			if (handler != null)
				onStop (handler);
		}

		public override void OnStart ()
		{
			base.OnStart ();

			if (handler != null) {
				onStart (handler);
				handler (this, null);
			}
		}
	}
}