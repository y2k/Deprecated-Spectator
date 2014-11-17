using System;
using Android.App;
using Gcm.Client;
using Android.Content;

namespace Spectator.Android.Application.Model.Gcm
{
	[Service] //Must use the service tag
	public class GcmService : GcmServiceBase
	{
		public GcmService () : base (GcmBroadcastReceiver.SENDER_IDS)
		{
		}

		protected override void OnRegistered (Context context, string registrationId)
		{
			//Receive registration Id for sending GCM Push Notifications to
		}

		protected override void OnUnRegistered (Context context, string registrationId)
		{
			//Receive notice that the app no longer wants notifications
		}

		protected override void OnMessage (Context context, Intent intent)
		{
			//Push Notification arrived - print out the keys/values
			if (intent == null || intent.Extras == null)
				foreach (var key in intent.Extras.KeySet())
					Console.WriteLine ("Key: {0}, Value: {1}", key, intent.Extras.GetString (key));
		}

		protected override bool OnRecoverableError (Context context, string errorId)
		{
			//Some recoverable error happened
		}

		protected override void OnError (Context context, string errorId)
		{
			//Some more serious error happened
		}
	}
}