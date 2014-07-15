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
        private IWebConnect web = ServiceLocator.Current.GetInstance<IWebConnect>();

		public ResultTask<IEnumerable<Subscription>> GetAllAsync (bool loadFromWeb)
		{
			if (loadFromWeb) {
				return ResultTask.Run<IEnumerable<Subscription>>(() =>
					{
						new ManualResetEvent(false).WaitOne(2000); // FIXME
						var data = web.Get<ProtoSubscriptionResponse>("api/subscription");

						var conn = ConnectionOpenHelper.Current;
						conn.SafeExecute("DELETE FROM subscriptions");
						var subs = data.Subscriptions.Select(s => new Subscription
							{
								ServerId = s.SubscriptionId,
								Title = s.Title,
								ThumbnailImageId = s.Thumbnail,
								GroupTitle = s.Group,
								UnreadCount = s.UnreadCount,
								Source = s.Source,
							});
						conn.SafeInsertAll(subs);

						return conn.SafeQuery<Subscription>("SELECT * FROM subscriptions ORDER BY GroupTitle, Title");
					});
			} else {
				return ResultTask.Run<IEnumerable<Subscription>>(() =>
					{
						var conn = ConnectionOpenHelper.Current;
						return conn.SafeQuery<Subscription>("SELECT * FROM subscriptions ORDER BY GroupTitle, Title");
					});
			}
		}

        public Task<IEnumerable<Subscription>> GetSubscriptionsAsync()
        {
            throw new NotImplementedException();
        }

        public ResultTask<IEnumerable<Subscription>> GetAllFromCacheAsync()
        {
            return ResultTask.Run<IEnumerable<Subscription>>(() =>
            {
                var conn = ConnectionOpenHelper.Current;
                return conn.SafeQuery<Subscription>("SELECT * FROM subscriptions ORDER BY GroupTitle, Title");
            });
        }

        public ResultTask<IEnumerable<Subscription>> GetAllAsync()
        {
            return ResultTask.Run<IEnumerable<Subscription>>(() =>
            {
				new ManualResetEvent(false).WaitOne(2000); // FIXME
                var data = web.Get<ProtoSubscriptionResponse>("api/subscription");

                var conn = ConnectionOpenHelper.Current;
                conn.SafeExecute("DELETE FROM subscriptions");
                var subs = data.Subscriptions.Select(s => new Subscription
                {
                    ServerId = s.SubscriptionId,
                    Title = s.Title,
                    ThumbnailImageId = s.Thumbnail,
                    GroupTitle = s.Group,
                    UnreadCount = s.UnreadCount,
                    Source = s.Source,
                });
                conn.SafeInsertAll(subs);

                return conn.SafeQuery<Subscription>("SELECT * FROM subscriptions ORDER BY GroupTitle, Title");
            });
        }

        //public void GetAllObserver(object receiver, Action<Result<IEnumerable<Subscription>>> callback)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UnRegisterReceiver(object receiver)
        //{
        //    throw new NotImplementedException();
        //}


        public event EventHandler<Result<IEnumerable<Subscription>>> SubscriptionChanged;

        public async void ReloadList()
        {
			SubscriptionChanged (this, await GetAllFromCacheAsync ());
			SubscriptionChanged (this, await GetAllAsync ());
        }

		public event EventHandler SubscriptionsChagned;
    }
}