using Gcm.Client;
using Android.App;
using Android.Content;

namespace Spectator.Droid.Platform.Gcm
{
	[BroadcastReceiver (Permission = Constants.PERMISSION_GCM_INTENTS)]
	[IntentFilter (
		new [] { Constants.INTENT_FROM_GCM_MESSAGE }, 
		Categories = new [] { "@PACKAGE_NAME@" })]
	[IntentFilter (
		new [] { Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, 
		Categories = new [] { "@PACKAGE_NAME@" })]
	[IntentFilter (
		new [] { Constants.INTENT_FROM_GCM_LIBRARY_RETRY },
		Categories = new [] { "@PACKAGE_NAME@" })]
	public class GcmBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
	{
		public static string[] SENDER_IDS = { "445037560545" };
	}
}