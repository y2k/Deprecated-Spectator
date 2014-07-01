using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Spectator.Core.Model.Database;

namespace Spectator.Core.Model
{
	public interface ISnapshotCollectionModel
	{
		Task<IEnumerable<Snapshot>> GetAllAsync (int subscriptionId);
	}
}