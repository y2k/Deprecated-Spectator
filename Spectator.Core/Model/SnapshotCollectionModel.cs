using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Spectator.Core.Model.Exceptions;
using Spectator.Core.Model.Web;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Web.Proto;
using System.Linq;
using Spectator.Core.Model.Database;

namespace Spectator.Core.Model
{
	public class SnapshotCollectionModel : ISnapshotCollectionModel
	{
		private IWebConnect web = ServiceLocator.Current.GetInstance<IWebConnect> ();

		[Obsolete]
		private static readonly IDictionary<long, IEnumerable<Snapshot>> Cache = new Dictionary<long, IEnumerable<Snapshot>> ();

		#region ISnapshotCollectionModel implementation

		public Task<IEnumerable<Snapshot>> GetAllAsync (long subscriptionId)
		{
			return Task.Run<IEnumerable<Snapshot>> (() => {
				var url = subscriptionId == 0
					? "api/snapshot2"
					: "api/snapshot2?subId=" + subscriptionId;
				var data = web.Get<ProtoSnapshotsResponse> (url);
				return data.Snapshots.Select (s => new Snapshot{ Title = s.Title, ThumbnailImageId = s.Thumbnail }).ToList ();
			});
		}

		public event EventHandler<SnapshotChangedArgs> SnapshotChanged;

		public async void RequestSnapshots (long subscriptionId)
		{
			try {
				IEnumerable<Snapshot> s;
				if (Cache.TryGetValue(subscriptionId, out s)) 
					SnapshotChanged (this, new SnapshotChangedArgs { SubscriptionId = subscriptionId, Items = s, FromCache = true });
			} catch (Exception) {
				// SnapshotChanged (this, new SnapshotChangedArgs { SubscriptionId = subscriptionId, Error = e });
			}

			try {
				var s = await GetAllAsync (subscriptionId);
				Cache[subscriptionId] = s;
				SnapshotChanged (this, new SnapshotChangedArgs { SubscriptionId = subscriptionId, Items = s });
			} catch (Exception e) {
				SnapshotChanged (this, new SnapshotChangedArgs { SubscriptionId = subscriptionId, Error = e });
			}
		}

		#endregion
	}
}