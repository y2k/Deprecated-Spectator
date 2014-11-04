using System.Linq;
using Spectator.Core.Model.Web;
using Spectator.Core.Model.Database;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;

namespace Spectator.Core.Model.Account
{
	public class RepositoryAuthStorage : IAuthStorage
	{
		readonly IRepository repo = ServiceLocator.Current.GetInstance<IRepository>();

		#region ICookieProvider implementation

		public IDictionary<string, string> Load ()
		{
			return repo.GetCookies ().ToDictionary (s => s.Name, s => s.Value);
		}

		public void Save (IDictionary<string, string> authState)
		{
			repo.ReplaceAll (authState.Select (s => new AccountCookie { Name = s.Key, Value = s.Value }));
		}

		#endregion
	}
}