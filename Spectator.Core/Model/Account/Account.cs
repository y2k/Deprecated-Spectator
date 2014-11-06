using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Web;

namespace Spectator.Core.Model.Account
{
	public class Account
	{
		readonly ISpectatorApi web = ServiceLocator.Current.GetInstance<ISpectatorApi> ();
		readonly IRepository repo = ServiceLocator.Current.GetInstance<IRepository> ();

		public Task LoginByCode (string code)
		{
			return Task.Run (() => {
				var state = web.LoginByCode (code);
				repo.ReplaceAll (state.Select (s => new AccountCookie { Name = s.Key, Value = s.Value }));
			});
		}

		public Task Logout ()
		{
			return Task.Run (() => repo.ReplaceAll (new AccountCookie[0]));
		}
	}
}