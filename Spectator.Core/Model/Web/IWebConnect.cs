using System;

namespace Spectator.Core.Model.Web
{
	public interface IWebConnect
	{
		T Get<T> (string url);

		void PostWebForm (string url, params object[] formKeyValues);
	}
}