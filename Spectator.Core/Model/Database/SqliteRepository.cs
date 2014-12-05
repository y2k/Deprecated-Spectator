using SQLite.Net;
using System.Collections.Generic;
using System.Linq;

namespace Spectator.Core.Model.Database
{
	public class SqliteRepository : IRepository
	{
		readonly SQLiteConnection db;

		public SqliteRepository (SQLiteConnection db)
		{
			this.db = db;
		}

		public SqliteRepository () : this (ConnectionOpenHelper.Current)
		{
		}

		public Subscription GetSubscription (int id)
		{
			return db.SafeQuery<Subscription> ("SELECT * FROM subscriptions WHERE Id = ?", id).First ();
		}

		public List<Subscription> GetSubscriptions ()
		{
			return db.SafeQuery<Subscription> ("SELECT * FROM subscriptions ORDER BY GroupTitle, Title");
		}

		public void DeleteAllSnapshots (int subscriptionId)
		{
			db.SafeRunInTransaction (() => {
				db.SafeExecute ("DELETE FROM attachments WHERE SnapshotId IN (SELECT Id FROM snapshots WHERE SubscriptionId = ?)", subscriptionId);
				db.SafeExecute ("DELETE FROM snapshots WHERE SubscriptionId = ?", subscriptionId);
			});
		}

		public void Add (int subscriptionId, IEnumerable<Snapshot> snapshots)
		{
			db.SafeRunInTransaction (() => {
				foreach (var s in snapshots) {
					s.SubscriptionId = subscriptionId;
					db.SafeInsert (s);
				}
			});
		}

		public List<Snapshot> GetSnapshots (int subscriptionId)
		{
			return db.SafeQuery<Snapshot> ("SELECT * FROM snapshots WHERE SubscriptionId = ? ORDER BY rowid", subscriptionId);
		}

		public Snapshot GetSnapshot (int id)
		{
			return db.SafeQuery<Snapshot> ("SELECT * FROM snapshots WHERE Id = ?", id).First ();
		}

		public void Update (Snapshot snapshot)
		{
			db.SafeUpdate (snapshot);
		}

		public IEnumerable<Attachment> GetAttachements (int snapshotId)
		{
			return db.SafeQuery<Attachment> ("SELECT * FROM attachments WHERE SnapshotId = ?", snapshotId);
		}

		public void ReplaceAll (IEnumerable<AccountCookie> cookies)
		{
			db.SafeRunInTransaction (() => {
				db.SafeExecute ("DELETE FROM cookies");
				foreach (var s in cookies)
					db.SafeInsert (s);
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
				foreach (var s in subscriptions)
					db.SafeInsert (s);
			});
		}

		public void ReplaceAll (IEnumerable<Attachment> attachments)
		{
			db.SafeRunInTransaction (() => {
				db.SafeExecute ("DELETE FROM attachments");
				foreach (var s in attachments)
					db.SafeInsert (s);
			});
		}
	}
}