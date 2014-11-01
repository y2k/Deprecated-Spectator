using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using System.Collections.Generic;

namespace Spectator.Core.Model.Database
{
	public class SqliteRepository : IRepository
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
	}
}