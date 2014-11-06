using System;
using System.Threading.Tasks;

namespace Spectator.Core.Model
{
	public class SubscriptionOperations
	{
		public SubscriptionOperations ()
		{
		}

		public Task CreateNew (Uri link, string title)
		{
			throw new NotImplementedException ();
		}

		public Task Delete (int subscriptionId)
		{
			throw new NotImplementedException ();
		}

		public Task Edit (int subscriptionId, string title)
		{
			throw new NotImplementedException ();
		}
	}
}