using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Web;

namespace Spectator.Core.Model.Push
{
	public class PushModel
	{
		ISpectatorApi api = ServiceLocator.Current.GetInstance<ISpectatorApi> ();
		INotificationService notifService = ServiceLocator.Current.GetInstance<INotificationService> ();
		SubscriptionCollectionModel subsModel = new SubscriptionCollectionModel ();

		public void HandleNewUserToken (string token, PushPlatform platform)
		{
			Task.Run (() => api.SendPushToken (token, (int)platform));
		}

		public async Task HandleNewSyncMessage ()
		{
			await subsModel.Reload ();
			var subs = await subsModel.Get ();
			notifService.ShowHasNewMessage (
				subs.Count (s => s.UnreadCount > 0),
				subs.Sum (s => s.UnreadCount));
		}

		public enum PushPlatform
		{
			Ios = 1,
			Android = 2,
			WindowsPhone = 3,
		}
	}
}