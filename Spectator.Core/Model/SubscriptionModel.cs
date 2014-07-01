using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spectator.Core.Model.Tasks;
using Spectator.Core.Model.Exceptions;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Web;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Web.Proto;

namespace Spectator.Core.Model
{
	class SubscriptionModel : ISubscriptionModel
	{
		private IWebConnect web = ServiceLocator.Current.GetInstance<IWebConnect> ();

		public Task<IEnumerable<Subscription>> GetSubscriptionsAsync ()
		{
			throw new NotImplementedException ();
		}

		public ResultTask<IEnumerable<Subscription>> GetAllAsync ()
		{
			return ResultTask.Run<IEnumerable<Subscription>> (() => {
				var data = web.Get<ProtoSubscriptionResponse> ("http://debug.spectator.api-i-twister.net/api/subscription2");
				return data.Subscriptions.Select (s => new Subscription{ Title = s.Title, ThumbnailImageId = s.Thumbnail }).ToList ();
			});
		}
	}
}