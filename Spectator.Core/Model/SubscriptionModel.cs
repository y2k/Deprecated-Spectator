using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spectator.Core.Model.Tasks;
using Spectator.Core.Model.Exceptions;

namespace Spectator.Core.Model
{
	class SubscriptionModel : ISubscriptionModel
    {
        public Task<IEnumerable<object>> GetSubscriptionsAsync()
        {
            return Task.Run<IEnumerable<object>>(() => {
                new ManualResetEvent(false).WaitOne(1000);
                if (true) throw new WrongAuthException();
                return new object[] { "one", "two", "free" };
            });
        }

		public ResultTask<IEnumerable<object>> GetAllAsync ()
		{
			return ResultTask.Run<IEnumerable<object>> (() => {
				new ManualResetEvent(false).WaitOne(1000);
				if (true) throw new WrongAuthException();
				return new object[] { "one", "two", "free" };
			});
		}
    }
}