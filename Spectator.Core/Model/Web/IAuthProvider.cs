using System.Collections.Generic;

namespace Spectator.Core.Model.Web
{
	public interface IAuthProvider
	{
		IDictionary<string,string> Load();
	}
}