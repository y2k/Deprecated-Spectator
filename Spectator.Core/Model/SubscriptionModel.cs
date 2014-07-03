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

		public ResultTask<IEnumerable<Subscription>> GetAllFromCacheAsync ()
		{
			return ResultTask.Run<IEnumerable<Subscription>> (() => {
				var conn = ConnectionOpenHelper.Current;
				return conn.SafeQuery<Subscription>("SELECT * FROM subscriptions ORDER BY GroupTitle, Title");
			});
		}

		public ResultTask<IEnumerable<Subscription>> GetAllAsync ()
		{
			return ResultTask.Run<IEnumerable<Subscription>> (() => {
				var data = web.Get<ProtoSubscriptionResponse> ("http://debug.spectator.api-i-twister.net/api/subscription2");

				var conn = ConnectionOpenHelper.Current;
				conn.SafeExecute ("DELETE FROM subscriptions");
				var subs = data.Subscriptions.Select (s => new Subscription { 
					ServerId = s.SubscriptionId, 
					Title = s.Title, 
					ThumbnailImageId = s.Thumbnail,
					GroupTitle = s.Group, 
					UnreadCount = s.UnreadCount,
					Source = s.Source,
				});
				conn.SafeInsertAll (subs);

				return conn.SafeQuery<Subscription>("SELECT * FROM subscriptions ORDER BY GroupTitle, Title");
			});
		}
	}
}