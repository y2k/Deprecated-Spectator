using System;
using System.Collections.Generic;
using Spectator.Core.Model.Web.Proto;

namespace Spectator.Core.Model.Web
{
	public interface ISpectatorApi
	{
		SnapshotsResponse GetSnapshots (int bottomId);

		SnapshotsResponse GetSnapshots (int subscriptionId, int bottomId);

		SnapshotsResponse.ProtoSnapshot GetSnapshot (int serverId);

		SubscriptionResponse GetSubscriptions ();

		IDictionary<string,string> LoginByCode (string code);

		void CreateSubscription (Uri link, string title);

		void DeleteSubscription (int id);

		void EditSubscription (int id, string title);
	}
}