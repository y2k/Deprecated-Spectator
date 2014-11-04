using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Web;
using Spectator.Core.Model.Web.Proto;
using System.Threading;

namespace Spectator.Core.Model
{
	public class SnapshotCollectionModel
	{
		private const int FeedSubscriptionId = 0;

		IApiClient web = ServiceLocator.Current.GetInstance<IApiClient> ();
		IRepository repo = ServiceLocator.Current.GetInstance<IRepository> ();

		int subscriptionId;

		public SnapshotCollectionModel (int subscriptionId)
		{
			this.subscriptionId = subscriptionId;
		}

		public Task<IEnumerable<Snapshot>> Get ()
		{
			return Task.Run<IEnumerable<Snapshot>> (() => repo.GetSnapshots (subscriptionId));
		}

		public Task Next ()
		{
			return Task.Run (() => {
				int bottomId = GetBottomId ();
				var data = subscriptionId == FeedSubscriptionId
					? web.GetSnapshots (bottomId) 
					: web.GetSnapshots (subscriptionId, bottomId);
				repo.Add (subscriptionId, data.Snapshots.Select (s => s.ConvertToSnapshot (subscriptionId)));
			});
		}

		int GetBottomId ()
		{
			return repo.GetSnapshots (subscriptionId).Select (s => s.ServerId).LastOrDefault ();
		}

		public Task Reset ()
		{
			return Task.Run (() => repo.Delete (subscriptionId));
		}

		public static SnapshotCollectionModel CreateForFeed ()
		{
			return new SnapshotCollectionModel (FeedSubscriptionId);
		}
	}
}