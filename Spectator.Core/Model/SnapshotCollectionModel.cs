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
		[Obsolete]
		static readonly IDictionary<long, IEnumerable<Snapshot>> Cache = new Dictionary<long, IEnumerable<Snapshot>> ();

		IApiClient web = ServiceLocator.Current.GetInstance<IApiClient> ();
		IRepository storage = ServiceLocator.Current.GetInstance<IRepository> ();

		int subscriptionId;

		public SnapshotCollectionModel (int subscriptionId)
		{
			this.subscriptionId = subscriptionId;
		}

		public Task<IEnumerable<Snapshot>> Next ()
		{
			return Task.Run<IEnumerable<Snapshot>> (() => {
				var data = subscriptionId == 0 
					? web.Get (0) 
					: web.Get (subscriptionId, 0);
				storage.ReplaceAll (subscriptionId, data.Snapshots.Select (s => ConvertToSnapshot (subscriptionId, s)));
				return storage.GetAll ();
			});
		}

		static Snapshot ConvertToSnapshot (int subscriptionId, SnapshotsResponse.ProtoSnapshot s)
		{
			return s.ConvertToSnapshot (subscriptionId);
		}

		public Task Reset ()
		{
			return Task.Delay (1000);
		}

		#region Old methods

		public Task<IEnumerable<Snapshot>> GetAllAsync (bool loadFromWeb, int subscriptionId)
		{
//			if (loadFromWeb) {
//				return Task.Run<IEnumerable<Snapshot>> (() => {
//					var url = subscriptionId == 0
//                        ? "api/snapshot"
//                        : "api/snapshot?subId=" + subscriptionId;
//
//					var data = web.Get<SnapshotsResponse> (url);
//
//					storage.ReplaceAll (subscriptionId, data.Snapshots.Select (s => ConvertToSnapshot (subscriptionId, s)));
//					return storage.GetAll ();
//				});
//			}
//
//			return Task.Run<IEnumerable<Snapshot>> (() => storage.GetAll ());
			return null;
		}

		public Task<IEnumerable<Snapshot>> GetAllAsync (long subscriptionId)
		{
//			return Task.Run<IEnumerable<Snapshot>> (() => {
//				var url = subscriptionId == 0
//                    ? "api/snapshot"
//                    : "api/snapshot?subId=" + subscriptionId;
//
//				// new ManualResetEvent(false).WaitOne(2000); // FIXME
//				var data = web.Get<SnapshotsResponse> (url);
//				return data.Snapshots.Select (s => new Snapshot { 
//					Title = s.Title, 
//					ThumbnailWidth = s.ThumbnailWidth,
//					ThumbnailHeight = s.ThumbnailHeight,
//					ThumbnailImageId = s.Thumbnail
//				}).ToList ();
//			});
			return null;
		}

		public void RequestSnapshots (long subscriptionId)
		{
//			try {
//				IEnumerable<Snapshot> s;
//				if (Cache.TryGetValue (subscriptionId, out s))
//					SnapshotChanged (this, new SnapshotChangedArgs {
//						SubscriptionId = subscriptionId,
//						Items = s,
//						FromCache = true
//					});
//			} catch (Exception) {
//				// SnapshotChanged (this, new SnapshotChangedArgs { SubscriptionId = subscriptionId, Error = e });
//			}
//
//			try {
//				var s = await GetAllAsync (subscriptionId);
//				Cache [subscriptionId] = s;
//				SnapshotChanged (this, new SnapshotChangedArgs { SubscriptionId = subscriptionId, Items = s });
//			} catch (Exception e) {
//				SnapshotChanged (this, new SnapshotChangedArgs { SubscriptionId = subscriptionId, Error = e });
//			}
		}

		#endregion
	}
}