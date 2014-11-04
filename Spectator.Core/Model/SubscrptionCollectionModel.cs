using System;
using System.Collections.Generic;
using Spectator.Core.Model.Database;
using System.Threading.Tasks;

namespace Spectator.Core.Model
{
	public class SubscrptionCollectionModel
	{
		public SubscrptionCollectionModel ()
		{
		}

		public Task<IEnumerable<Subscription>> Get ()
		{
			throw new NotImplementedException ();
		}

		public Task Reload ()
		{
			throw new NotImplementedException ();
		}
	}
}