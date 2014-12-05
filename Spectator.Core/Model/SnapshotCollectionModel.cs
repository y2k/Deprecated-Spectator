using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Web;
using Spectator.Core.Model.Web.Proto;

namespace Spectator.Core.Model
{
	public class SnapshotCollectionModel
	{
		const int FeedSubscriptionId = 0;

		ISpectatorApi web = ServiceLocator.Current.GetInstance<ISpectatorApi> ();
		IRepository repo = ServiceLocator.Current.GetInstance<IRepository> ();

		int subscriptionId;

		public int SubscriptionId { get { return subscriptionId; } }

		public SnapshotCollectionModel (int subscriptionId)
		{
			this.subscriptionId = subscriptionId;
		}

		public Task<List<Snapshot>> Get ()
		{
			return Task.Run (() => repo.GetSnapshots (subscriptionId));
		}

		public Task Next ()
		{
			return Task.Run (() => {
				int bottomId = GetBottomId ();
				var data = subscriptionId == FeedSubscriptionId
					? web.GetSnapshots (bottomId) 
					: web.GetSnapshots (GetServerSubscriptionId (), bottomId);
				repo.Add (subscriptionId, data.Snapshots.Select (s => s.ConvertToSnapshot ()));
			});
		}

		int GetBottomId ()
		{
			return repo.GetSnapshots (subscriptionId).Select (s => s.ServerId).LastOrDefault ();
		}

		int GetServerSubscriptionId ()
		{
			return (int)repo.GetSubscription (subscriptionId).ServerId;
		}

		public Task Reset ()
		{
			return Task.Run (() => repo.DeleteAllSnapshots (subscriptionId));
		}

		public static SnapshotCollectionModel CreateForFeed ()
		{
			return new SnapshotCollectionModel (FeedSubscriptionId);
		}
	}
}