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

		ResultTask<IEnumerable<Subscription>> GetAllAsync(bool loadFromWeb);

		// ==========================================================

		event EventHandler SubscriptionsChagned;

        event EventHandler<Result<IEnumerable<Subscription>>> SubscriptionChanged;

		void ReloadList();
    }
}