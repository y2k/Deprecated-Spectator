using System;
using System.Linq;
using System.Collections.Generic;

namespace Spectator.Core.Model.Database
{
    public class MemoryRepository : IRepository
    {
        List<Subscription> subscriptions = new List<Subscription>();
        List<Snapshot> snapshots = new List<Snapshot>();
        List<Attachment> attachments = new List<Attachment>();
        //List<AccountCookie> cookies = new List<AccountCookie>();

        int subscriptionIndex;
        int snapshotIndex;
        int attachmentIndex;
        int cookieIndex;

        public void Add(int subscriptionId, IEnumerable<Snapshot> snapshots)
        {
            LockSelf(() =>
            {
                foreach (var s in snapshots)
                {
                    s.SubscriptionId = subscriptionId;
                    s.Id = snapshotIndex++;
                    this.snapshots.Add(s);
                }
            });
        }

        public void DeleteAllSnapshots(int subscriptionId)
        {
            LockSelf(() =>
            {
                var ids = snapshots.Where(s => s.SubscriptionId == subscriptionId).Select(s => s.Id).ToList();
                attachments.RemoveAll(s => ids.Contains(s.SnapshotId));
                snapshots.RemoveAll(s => s.SubscriptionId == subscriptionId);
            });
        }

        public IEnumerable<Attachment> GetAttachements(int snapshotId)
        {
            return LockSelf(() => attachments.Where(s => s.SnapshotId == snapshotId).ToList());
        }

        public Snapshot GetSnapshot(int id)
        {
            return LockSelf(() => snapshots.FirstOrDefault(s => s.Id == id));
        }

        public List<Snapshot> GetSnapshots(int subscriptionId)
        {
            return LockSelf(() => snapshots.Where(s => s.SubscriptionId == subscriptionId).ToList());
        }

        public Subscription GetSubscription(int id)
        {
            return LockSelf(() => subscriptions.FirstOrDefault(s => s.Id == id));
        }

        public List<Subscription> GetSubscriptions()
        {
            return LockSelf(() => subscriptions.ToList());
        }

        public void ReplaceAll(IEnumerable<Attachment> attachments)
        {
            LockSelf(() =>
            {
                this.attachments.Clear();
                foreach (var s in attachments)
                {
                    s.Id = attachmentIndex++;
                    this.attachments.Add(s);
                }
            });
        }

        public void ReplaceAll(IEnumerable<Subscription> subscriptions)
        {
            LockSelf(() =>
            {
                this.subscriptions.Clear();
                foreach (var s in subscriptions)
                {
                    s.Id = subscriptionIndex++;
                    this.subscriptions.Add(s);
                }
            });
        }

        public void Update(Snapshot snapshot)
        {
            LockSelf(() =>
            {
                snapshots.RemoveAll(s => s.Id == snapshot.Id);
                snapshots.Add(snapshot);
            });
        }

        void LockSelf(Action callback)
        {
            LockSelf(() =>
            {
                callback();
                return (object)null;
            });
        }

        T LockSelf<T>(Func<T> callback)
        {
            lock (this)
            {
                return callback();
            }
        }
    }
}