using System.Collections.Generic;

namespace Spectator.Core.Model.Database
{
	public interface IRepository
	{
		void ReplaceAll (int subscriptionId, IEnumerable<Snapshot> snapshots);

		IEnumerable<Snapshot> GetAll();

		Snapshot GetSnapshot (int id);

		void Update (Snapshot snapshot);

		IEnumerable<Attachment> GetAttachements (int snapshotId);

		void ReplaceAll(IEnumerable<AccountCookie> cookies);

		void ReplaceAll(IEnumerable<Subscription> subscriptions);

		IEnumerable<AccountCookie> GetCookies();

		IEnumerable<Subscription> GetSubscriptions ();
	}
}