using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Spectator.Core.Model.Database
{
    class MemoryRepository : IRepository
    {
        List<Subscription> subscriptions = new List<Subscription>();
        List<Snapshot> snapshots = new List<Snapshot>();
        List<Attachment> attachments = new List<Attachment>();
        List<AccountCookie> cookies = new List<AccountCookie>();

        public void Add(int subscriptionId, IEnumerable<Snapshot> snapshots)
        {
            LockSelf(() =>
            {
                foreach (var s in snapshots)
                {
                    s.SubscriptionId = subscriptionId;
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

        public IEnumerable<AccountCookie> GetCookies()
        {
            return LockSelf(() => cookies.ToList());
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
                var snapshotIds = attachments.Select(s => s.SnapshotId).Distinct().ToList();
                this.attachments.RemoveAll(s => snapshotIds.Contains(s.SnapshotId));
                this.attachments.AddRange(attachments);
            });
        }

        public void ReplaceAll(IEnumerable<Subscription> subscriptions)
        {
            LockSelf(() =>
            {
                

                this.subscriptions.AddRange(subscriptions);
            });
        }

        public void ReplaceAll(IEnumerable<AccountCookie> cookies)
        {
            LockSelf(() =>
            {
                this.cookies.Clear();
                this.cookies.AddRange(cookies);
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