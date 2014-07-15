using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Web;
using Spectator.Core.Model.Web.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Spectator.Core.Model
{
    public class SnapshotCollectionModel : ISnapshotCollectionModel
    {
        private IWebConnect web = ServiceLocator.Current.GetInstance<IWebConnect>();

        [Obsolete]
        private static readonly IDictionary<long, IEnumerable<Snapshot>> Cache = new Dictionary<long, IEnumerable<Snapshot>>();

        #region ISnapshotCollectionModel implementation

        public Task<IEnumerable<Snapshot>> GetAllAsync(bool loadFromWeb, int subscriptionId)
        {
            var db = ConnectionOpenHelper.Current;

            if (loadFromWeb)
            {
                return Task.Run<IEnumerable<Snapshot>>(() =>
                {
                    var url = subscriptionId == 0
                        ? "api/snapshot2"
                        : "api/snapshot2?subId=" + subscriptionId;

                    new ManualResetEvent(false).WaitOne(2000); // FIXME
                    var data = web.Get<ProtoSnapshotsResponse>(url);

                    db.SafeRunInTransaction(() =>
                    {
                        db.SafeExecute("DELETE FROM snapshots WHERE SubscriptionId = ?", subscriptionId);
                        db.SafeInsertAll(data.Snapshots.Select(s => new Snapshot { 
									SubscriptionId = subscriptionId, 
									Title = s.Title, 
									ThumbnailWidth = 3, // FIXME
									ThumbnailHeight = 2, // FIXME
									ThumbnailImageId = s.Thumbnail }));
                    });

                    return db.SafeQuery<Snapshot>("SELECT * FROM snapshots WHERE SubscriptionId = ? ORDER BY rowid");
                });
            }

            return Task.Run<IEnumerable<Snapshot>>(() => db.SafeQuery<Snapshot>("SELECT * FROM snapshots WHERE SubscriptionId = ? ORDER BY rowid"));
        }

        public Task<IEnumerable<Snapshot>> GetAllAsync(long subscriptionId)
        {
            return Task.Run<IEnumerable<Snapshot>>(() =>
            {
                var url = subscriptionId == 0
                    ? "api/snapshot2"
                    : "api/snapshot2?subId=" + subscriptionId;

                new ManualResetEvent(false).WaitOne(2000); // FIXME
                var data = web.Get<ProtoSnapshotsResponse>(url);
                return data.Snapshots.Select(s => new Snapshot { 
						Title = s.Title, 
						ThumbnailWidth = 3, // FIXME
						ThumbnailHeight = 2, // FIXME
						ThumbnailImageId = s.Thumbnail }).ToList();
            });
        }

        public event EventHandler<SnapshotChangedArgs> SnapshotChanged;

        public async void RequestSnapshots(long subscriptionId)
        {
            try
            {
                IEnumerable<Snapshot> s;
                if (Cache.TryGetValue(subscriptionId, out s))
                    SnapshotChanged(this, new SnapshotChangedArgs { SubscriptionId = subscriptionId, Items = s, FromCache = true });
            }
            catch (Exception)
            {
                // SnapshotChanged (this, new SnapshotChangedArgs { SubscriptionId = subscriptionId, Error = e });
            }

            try
            {
                var s = await GetAllAsync(subscriptionId);
                Cache[subscriptionId] = s;
                SnapshotChanged(this, new SnapshotChangedArgs { SubscriptionId = subscriptionId, Items = s });
            }
            catch (Exception e)
            {
                SnapshotChanged(this, new SnapshotChangedArgs { SubscriptionId = subscriptionId, Error = e });
            }
        }

        #endregion
    }
}