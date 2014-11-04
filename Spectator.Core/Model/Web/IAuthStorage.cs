using System.Collections.Generic;

namespace Spectator.Core.Model.Web
{
	public interface IAuthStorage
	{
		IDictionary<string,string> Load();

		void Save (IDictionary<string,string> authState);
	}
}