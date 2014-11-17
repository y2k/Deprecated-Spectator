using Android.Content;
using Gcm.Client;

namespace Spectator.Android.Application.Model.Gcm
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
		//IMPORTANT: Change this to your own Sender ID!
		//The SENDER_ID is your Google API Console App Project Number
		public static string[] SENDER_IDS = { "697360970929" };
	}
}