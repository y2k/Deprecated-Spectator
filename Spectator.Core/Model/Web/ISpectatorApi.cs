using System;
using System.Collections.Generic;
using Spectator.Core.Model.Web.Proto;
using System.Threading.Tasks;

namespace Spectator.Core.Model.Web
{
	public interface ISpectatorApi
	{
		Task SendPushToken (string userToken, int platformId);

		Task<SnapshotsResponse> GetSnapshots (int bottomId);

		Task<SnapshotsResponse> GetSnapshots (int subscriptionId, int bottomId);

		Task<SnapshotsResponse.ProtoSnapshot> GetSnapshot (int serverId);

		Task<SubscriptionResponse> GetSubscriptions ();

		Task<IDictionary<string, string>> LoginByCode (string code);

		Task CreateSubscription (Uri link, string title);

        Task DeleteSubscription (int id);

        Task EditSubscription (int id, string title);

        Uri CreateFullUrl(string relativePath);
	}
}