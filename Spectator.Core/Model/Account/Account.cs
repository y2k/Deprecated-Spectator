using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spectator.Core.Model.Account
{
	public class Account
	{
		readonly ISpectatorApi web = ServiceLocator.Current.GetInstance<ISpectatorApi> ();
		readonly IStorage repo = ServiceLocator.Current.GetInstance<IStorage> ();

		public Task LoginByCode (string code)
		{
			return Task.Run (async () => {
				var state = await web.LoginByCode (code);
				repo.ReplaceAll (state.Select (s => new AccountCookie { Name = s.Key, Value = s.Value }));
			});
		}

		public Task Logout ()
		{
			return Task.Run (() => repo.ReplaceAll (new AccountCookie[0]));
		}

        public interface IStorage
        {
            void ReplaceAll(IEnumerable<AccountCookie> cookies);
        }
    }
}