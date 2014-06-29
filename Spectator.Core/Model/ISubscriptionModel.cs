using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectator.Core.Model.Tasks;

namespace Spectator.Core.Model
{
    public interface ISubscriptionModel
    {
        Task<IEnumerable<object>> GetSubscriptionsAsync();

		ResultTask<IEnumerable<object>> GetAllAsync();

        //public class SubscriptionResult
        //{
        //    public const int StateSuccess = 0;
        //    public const int StateNotLogin = 1;
        //    public const int StateUnknownError = 2;
        //    public IEnumerable<object> Subscriptions { get; set; }
        //    public int status;
        //}
    }
}