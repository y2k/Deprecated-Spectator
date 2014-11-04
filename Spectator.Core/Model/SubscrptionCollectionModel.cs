using System.Linq;
using System.Collections.Generic;
using Spectator.Core.Model.Database;
using System.Threading.Tasks;
using Spectator.Core.Model.Web;
using Microsoft.Practices.ServiceLocation;

namespace Spectator.Core.Model
{
	public class SubscrptionCollectionModel
	{
		readonly IApiClient api = ServiceLocator.Current.GetInstance<IApiClient> ();
		readonly IRepository storage = ServiceLocator.Current.GetInstance<IRepository> ();

		public Task<IEnumerable<Subscription>> Get ()
		{
			return Task.Run (() => storage.GetSubscriptions ());
		}

		public Task Reload ()
		{
			return Task.Run (() => {
				var resp = api.GetSubscriptions ();
				var subs = resp.Subscriptions.Select (s => s.ConvertToSubscription ());
				storage.ReplaceAll (subs);
			});
		}
	}
}