using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectator.Core.Model.Database
{
    public interface IRepository
    {
        Subscription GetSubscription(int id);

        void DeleteAllSnapshots(int subscriptionId);

        void Add(int subscriptionId, IEnumerable<Snapshot> snapshots);

        List<Snapshot> GetSnapshots(int subscriptionId);

        [Obsolete]
        Snapshot GetSnapshot(int id);

        Task<Snapshot> GetSnapshotAsync(int id);

        void Update(Snapshot snapshot);

        IEnumerable<Attachment> GetAttachements(int snapshotId);

        void ReplaceAll(IEnumerable<Subscription> subscriptions);

        void ReplaceAll(IEnumerable<Attachment> attachments);

        List<Subscription> GetSubscriptions();
    }
}