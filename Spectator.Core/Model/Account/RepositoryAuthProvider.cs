using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Web;

namespace Spectator.Core.Model.Account
{
	public class RepositoryAuthProvider : IAuthProvider
	{
		readonly IRepository repo = ServiceLocator.Current.GetInstance<IRepository>();

		#region ICookieProvider implementation

		public IDictionary<string, string> Load ()
		{
			return repo.GetCookies ().ToDictionary (s => s.Name, s => s.Value);
		}

		#endregion
	}
}