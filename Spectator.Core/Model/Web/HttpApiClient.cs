using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Spectator.Core.Model.Web.Proto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Spectator.Core.Model.Web
{
    public class HttpApiClient : ISpectatorApi
	{
		readonly IAuthProvider authStorage = ServiceLocator.Current.GetInstance<IAuthProvider> ();

		#region IApiClient implementation

		public Task SendPushToken (string userToken, int platformId)
		{
			var form = new FormUrlEncodedContent (new [] {
				new KeyValuePair<string,string> ("RegistrationId", userToken),
				new KeyValuePair<string,string> ("PlatformId", "" + platformId),
			});
			var web = GetApiClient ();
            return web.client.PostAsync ("api/push/", form);
		}

		public Task EditSubscription (int id, string title)
		{
			var form = new FormUrlEncodedContent (new [] {
				new KeyValuePair<string,string> ("Title", title),
			});
			var web = GetApiClient ();
			return web.client.PostAsync ("api/subscription/" + id, form);
		}

		public Task DeleteSubscription (int id)
		{
            return GetApiClient().client.DeleteAsync("api/subscription/" + id);
		}

		public Task CreateSubscription (Uri link, string title)
		{
			var form = new FormUrlEncodedContent (new [] {
				new KeyValuePair<string,string> ("Source", link.AbsoluteUri),
				new KeyValuePair<string,string> ("Title", title),
			});
			var web = GetApiClient ();
			return web.client.PutAsync ("api/subscription", form);
		}

		public Task<SubscriptionResponse> GetSubscriptions ()
		{
			return DoGet<SubscriptionResponse> ("api/subscription");
		}

		public async Task<IDictionary<string,string>> LoginByCode (string code)
		{
			var web = GetApiClient ();
			var form = new [] { new KeyValuePair<string,string> ("code", code) };
			await web.client.PostAsync ("Account/LoginByCode", new FormUrlEncodedContent (form));
			return CookiesToDictionary (web.cookies);
		}

		IDictionary<string,string> CookiesToDictionary (CookieContainer cookies)
		{
			return cookies
				.GetCookies (Constants.BaseApi)
				.Cast<Cookie> ()
				.ToDictionary (s => s.Name, s => s.Value);
		}

		public Task<SnapshotsResponse.ProtoSnapshot> GetSnapshot (int serverId)
		{
			return DoGet<SnapshotsResponse.ProtoSnapshot> ("api/snapshot/" + serverId);
		}

		public Task<SnapshotsResponse> GetSnapshots (int toId)
		{
			var url = "api/snapshot";
			if (toId > 0)
				url = url + "?toId=" + toId;
			return DoGet<SnapshotsResponse> (url);
		}

		public Task<SnapshotsResponse> GetSnapshots (int subscriptionId, int toId)
		{
			return DoGet<SnapshotsResponse> ("api/snapshot?subId=" + subscriptionId);
		}

		#endregion

	    async Task<T> DoGet<T> (string url)
		{
			var r = await GetApiClient ().client.GetAsync (url);
			if (r.StatusCode == HttpStatusCode.Forbidden)
				throw new NotAuthException ();

			using (var s = await r.Content.ReadAsStreamAsync ())
				return (T)new JsonSerializer ().Deserialize (new StreamReader (s), typeof(T));
		}

		HttpClientHolder GetApiClient ()
		{
			var c = GetCookieContainer ();
			var client = new HttpClient (new HttpClientHandler {
				CookieContainer = c,
				UseCookies = true,
			}) { BaseAddress = Constants.BaseApi };
			return new HttpClientHolder { client = client, cookies = c };
		}

		CookieContainer GetCookieContainer ()
		{
			var c = new CookieContainer ();
			foreach (var s in authStorage.Load ())
				c.Add (Constants.BaseApi, new Cookie (s.Key, s.Value));
			return c;
		}

		class HttpClientHolder
		{
			internal HttpClient client;
			internal CookieContainer cookies;
		}
	}
}