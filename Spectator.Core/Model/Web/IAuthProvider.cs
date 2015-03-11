using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectator.Core.Model.Web
{
	public interface IAuthProvider
	{
		Task<IDictionary<string,string>> Load();
	}
}