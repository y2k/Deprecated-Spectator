using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using System.Collections.Generic;
using System.Linq;

namespace Spectator.Core.Model.Database
{
	internal class SqliteRepository : IRepository
	{
		readonly ISQLiteConnection db = ConnectionOpenHelper.Current;

		public void ReplaceAll (int subscriptionId, IEnumerable<Snapshot> snapshots)
		{
			db.SafeRunInTransaction (() => {
				db.SafeExecute ("DELETE FROM snapshots WHERE SubscriptionId = ?", subscriptionId);
				db.SafeInsertAll (snapshots);
			});
		}

		public IEnumerable<Snapshot> GetAll ()
		{
			return db.SafeQuery<Snapshot> ("SELECT * FROM snapshots WHERE SubscriptionId = ? ORDER BY rowid");
		}

		public Snapshot GetSnapshot (int id)
		{
			return db.SafeQuery<Snapshot> ("SELECT * FROM snapshots WHERE Id = ?", id).First ();
		}

		public void Update (Snapshot snapshot)
		{
			db.Update (snapshot);
		}

		public IEnumerable<Attachment> GetAttachements (int snapshotId)
		{
			return db.SafeQuery<Attachment> ("SELECT * FROM attachments WHERE SnapshotId = ?", snapshotId);
		}
	}
}