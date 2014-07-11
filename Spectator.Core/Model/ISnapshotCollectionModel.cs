using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Spectator.Core.Model.Database;

namespace Spectator.Core.Model
{
	public interface ISnapshotCollectionModel
	{
		[Obsolete]
		Task<IEnumerable<Snapshot>> GetAllAsync (long subscriptionId);

		Task<IEnumerable<Snapshot>> GetAllAsync (bool loadFromWeb, int subscriptionId);

		event EventHandler<SnapshotChangedArgs> SnapshotChanged;

		void RequestSnapshots(long subscriptionId);
	}

	public class SnapshotChangedArgs 
	{
		public long SubscriptionId { get; set; }
		public IEnumerable<Snapshot> Items { get; set; }
		public Exception Error { get; set; }
		public bool FromCache { get; set; }
	}
}