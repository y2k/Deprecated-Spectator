using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using System.Collections.Generic;
using System.Linq;

namespace Spectator.Core.Model.Database
{
	public class SqliteRepository : IRepository
	{
		readonly ISQLiteConnection db;

		public SqliteRepository (ISQLiteConnection db)
		{
			this.db = db;
		}

		public SqliteRepository () : this (ConnectionOpenHelper.Current)
		{
		}

		public IEnumerable<Subscription> GetSubscriptions ()
		{
			return db.SafeQuery<Subscription> ("SELECT * FROM subscriptions ORDER BY GroupTitle, Title");
		}

		public void Delete (int subscriptionId)
		{
			db.SafeExecute ("DELETE FROM snapshots WHERE SubscriptionId = ?", subscriptionId);
		}

		public void Add (int subscriptionId, IEnumerable<Snapshot> snapshots)
		{
			db.SafeInsertAll (snapshots);
		}

		public IEnumerable<Snapshot> GetSnapshots (int subscriptionId)
		{
			return db.SafeQuery<Snapshot> ("SELECT * FROM snapshots WHERE SubscriptionId = ? ORDER BY rowid", subscriptionId);
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

		public void ReplaceAll (IEnumerable<AccountCookie> cookies)
		{
			db.SafeRunInTransaction (() => {
				db.SafeExecute ("DELETE FROM cookies");
				db.SafeInsertAll (cookies);
			});
		}

		public IEnumerable<AccountCookie> GetCookies ()
		{
			return db.SafeQuery<AccountCookie> ("SELECT * FROM cookies");
		}

		public void ReplaceAll (IEnumerable<Subscription> subscriptions)
		{
			db.SafeRunInTransaction (() => {
				db.SafeExecute ("DELETE FROM subscriptions");
				db.SafeInsertAll (subscriptions);
			});
		}
	}
}