using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Spectator.Core.Model
{
	public interface ISnapshotCollectionModel
	{
		Task<IEnumerable<object>> GetAllAsync (int subscriptionId);
	}
}