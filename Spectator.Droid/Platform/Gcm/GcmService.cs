using Android.App;
using Android.Content;
using Gcm.Client;
using Spectator.Core.Model.Push;

namespace Spectator.Droid.Platform.Gcm
{
	[Service]
	public class GcmService : GcmServiceBase
	{
		PushModel model = new PushModel ();

		public GcmService () : base (GcmBroadcastReceiver.SENDER_IDS)
		{
		}

		protected override void OnRegistered (Context context, string registrationId)
		{
			model.HandleNewUserToken (registrationId, PushModel.PushPlatform.Android);
		}

		protected override void OnUnRegistered (Context context, string registrationId)
		{
			// TODO Добавить обработку отписывания от GCM
		}

		protected override void OnMessage (Context context, Intent intent)
		{
			model.HandleNewSyncMessage ().Wait ();
		}

		protected override bool OnRecoverableError (Context context, string errorId)
		{
			return false; // Ignore
		}

		protected override void OnError (Context context, string errorId)
		{
			// Ignore
		}
	}
}