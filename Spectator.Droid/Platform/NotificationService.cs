using Spectator.Core.Model.Push;
using Android.Content;
using Android.App;
using Android.Support.V4.App;

namespace Spectator.Droid.Platform
{
	public class NotificationService : INotificationService
	{
		public void ShowHasNewMessage (int updatedSubscriptions, int totalNewShapshots)
		{
			var nm = (NotificationManager)App.Current.GetSystemService (Context.NotificationService);

			var style = new NotificationCompat.BigTextStyle ();
			style.BigText (
				"Updated subscriptions: " + updatedSubscriptions +
				"\nTotal new snapshots: " + totalNewShapshots);

			var b = new NotificationCompat.Builder (App.Current);
			b.SetSmallIcon (Resource.Drawable.Icon);
			b.SetNumber (updatedSubscriptions);
			b.SetStyle (style);

			nm.Notify (0, b.Build ());
		}
	}
}