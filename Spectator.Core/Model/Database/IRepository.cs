using System.Collections.Generic;

namespace Spectator.Core.Model.Database
{
	public interface IRepository
	{
		Subscription GetSubscription (int id);

		void DeleteAllSnapshots (int subscriptionId);

		void Add (int subscriptionId, IEnumerable<Snapshot> snapshots);

		List<Snapshot> GetSnapshots(int subscriptionId);

		Snapshot GetSnapshot (int id);

		void Update (Snapshot snapshot);

		IEnumerable<Attachment> GetAttachements (int snapshotId);

		void ReplaceAll(IEnumerable<Subscription> subscriptions);

		void ReplaceAll(IEnumerable<Attachment> attachments);

		List<Subscription> GetSubscriptions ();
	}
}