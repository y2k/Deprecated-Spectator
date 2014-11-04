using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Web;
using Spectator.Core.Model.Database;

namespace Spectator.Core.Model.Account
{
	public class Account
	{
		readonly IApiClient web = ServiceLocator.Current.GetInstance<IApiClient> ();
		readonly IRepository repo = ServiceLocator.Current.GetInstance<IRepository> ();

		public Task LoginByCode (string code)
		{
			return Task.Run (() => web.LoginByCode (code));
		}

		public Task Logout ()
		{
			return Task.Run (() => repo.ReplaceAll (new AccountCookie[0]));
		}
	}
}