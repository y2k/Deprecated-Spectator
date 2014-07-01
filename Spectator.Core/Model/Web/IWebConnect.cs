using System;

namespace Spectator.Core.Model.Web
{
	public interface IWebConnect
	{
		void LoadSnapshots(int id);

		void PostWebForm (string url, params object[] formKeyValues);
	}
}