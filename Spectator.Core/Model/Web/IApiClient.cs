using System;
using Spectator.Core.Model.Web.Proto;
using System.Collections.Generic;

namespace Spectator.Core.Model.Web
{
	public interface IApiClient
	{
		[Obsolete]
		T GetSnapshots<T> (string url);

		SnapshotsResponse GetSnapshots (int bottomId);

		SnapshotsResponse GetSnapshots (int subscriptionId, int bottomId);

		SnapshotsResponse.ProtoSnapshot GetSnapshot (int serverId);

		SubscriptionResponse GetSubscriptions ();

		IDictionary<string,string> LoginByCode (string code);
	}
}