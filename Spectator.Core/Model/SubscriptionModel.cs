using System;
using System.Linq;
using System.Threading.Tasks;
using Spectator.Core.Model.Web;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Database;

namespace Spectator.Core.Model
{
	public class SubscriptionModel
	{
		readonly ISpectatorApi api = ServiceLocator.Current.GetInstance<ISpectatorApi> ();
		readonly IRepository repo = ServiceLocator.Current.GetInstance<IRepository> ();

		public Task CreateNew (Uri link, string title)
		{
			return Task.Run (() => api.CreateSubscription (link, title));
		}

		public Task Delete (int subscriptionId)
		{
			return Task.Run (() => {
				var sub = repo.GetSubscriptions ().First (s => s.Id == subscriptionId);
				api.DeleteSubscription ((int)sub.ServerId);
			});
		}

		public Task Edit (int subscriptionId, string title)
		{
			return Task.Run (() => {
				var sub = repo.GetSubscriptions ().First (s => s.Id == subscriptionId);
				api.EditSubscription ((int)sub.ServerId, title);
			});
		}
	}
}