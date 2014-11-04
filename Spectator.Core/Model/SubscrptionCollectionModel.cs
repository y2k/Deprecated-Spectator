using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Web;

namespace Spectator.Core.Model
{
	public class SubscrptionCollectionModel
	{
		readonly IApiClient api = ServiceLocator.Current.GetInstance<IApiClient> ();
		readonly IRepository storage = ServiceLocator.Current.GetInstance<IRepository> ();

		public Task<IEnumerable<Subscription>> Get ()
		{
			return Task.Run (() => {
				return storage.GetSubscriptions ();
			});
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