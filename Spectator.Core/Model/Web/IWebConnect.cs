using System;
using Spectator.Core.Model.Web.Proto;

namespace Spectator.Core.Model.Web
{
	public interface IWebConnect
	{
		[Obsolete]
		T Get<T> (string url);

		SnapshotsResponse Get (int toId);

		SnapshotsResponse Get (int subscriptionId, int toId);

		void PostWebForm (string url, params object[] formKeyValues);
	}
}