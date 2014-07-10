using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectator.Core.Model.Tasks;
using Spectator.Core.Model.Database;

namespace Spectator.Core.Model
{
    public interface ISubscriptionModel
	{
		[Obsolete]
        Task<IEnumerable<Subscription>> GetSubscriptionsAsync();

		[Obsolete]
        ResultTask<IEnumerable<Subscription>> GetAllFromCacheAsync();

		[Obsolete]
        ResultTask<IEnumerable<Subscription>> GetAllAsync();

        //public class SubscriptionResult
        //{
        //    public const int StateSuccess = 0;
        //    public const int StateNotLogin = 1;
        //    public const int StateUnknownError = 2;
        //    public IEnumerable<object> Subscriptions { get; set; }
        //    public int status;
        //}


        // ==========================================================

        event EventHandler<Result<IEnumerable<Subscription>>> SubscriptionChanged;
        void ReloadList();

        //void GetAllObserver(object receiver, Action<Result<IEnumerable<Subscription>>> callback);
        //void UnRegisterReceiver(object receiver);
    }
}