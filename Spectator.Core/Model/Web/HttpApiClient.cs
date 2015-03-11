using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
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
        static readonly Uri BaseApi = new Uri("http://remote-cache-3.api-i-twister.net/");
        readonly IAuthProvider authStorage = ServiceLocator.Current.GetInstance<IAuthProvider>();

        #region IApiClient implementation

        public Uri CreateFullUrl(string relativePath)
        {
            return  new Uri(BaseApi, relativePath);
        }

        public Task SendPushToken(string userToken, int platformId)
        {
            var form = new FormUrlEncodedContent(new []
                {
                    new KeyValuePair<string,string>("RegistrationId", userToken),
                    new KeyValuePair<string,string>("PlatformId", "" + platformId),
                });
            var web = GetApiClient();
            return web.client.PostAsync("api/push/", form);
        }

        public Task EditSubscription(int id, string title)
        {
            var form = new FormUrlEncodedContent(new []
                {
                    new KeyValuePair<string,string>("Title", title),
                });
            var web = GetApiClient();
            return web.client.PostAsync("api/subscriptions/" + id, form);
        }

        public Task DeleteSubscription(int id)
        {
            return GetApiClient().client.DeleteAsync("api/subscriptions/" + id);
        }

        public Task CreateSubscription(Uri link, string title)
        {
            var form = new FormUrlEncodedContent(new []
                {
                    new KeyValuePair<string,string>("Source", link.AbsoluteUri),
                    new KeyValuePair<string,string>("Title", title),
                });
            var web = GetApiClient();
            return web.client.PutAsync("api/subscriptions", form);
        }

        public Task<SubscriptionResponse> GetSubscriptions()
        {
            return DoGet<SubscriptionResponse>("api/subscriptions");
        }

        public async Task<IDictionary<string,string>> LoginByCode(string code)
        {
            var web = GetApiClient();
            var form = new [] { new KeyValuePair<string,string>("code", code) };
            await web.client.PostAsync("Account/LoginByCode", new FormUrlEncodedContent(form));
            return CookiesToDictionary(web.cookies);
        }

        IDictionary<string,string> CookiesToDictionary(CookieContainer cookies)
        {
            return cookies
				.GetCookies(BaseApi)
				.Cast<Cookie>()
				.ToDictionary(s => s.Name, s => s.Value);
        }

        public Task<SnapshotsResponse.ProtoSnapshot> GetSnapshot(int serverId)
        {
            return DoGet<SnapshotsResponse.ProtoSnapshot>("api/snapshots/" + serverId);
        }

        public Task<SnapshotsResponse> GetSnapshots(int toId)
        {
            var url = "api/snapshots";
            if (toId > 0)
                url = url + "?toId=" + toId;
            return DoGet<SnapshotsResponse>(url);
        }

        public Task<SnapshotsResponse> GetSnapshots(int subscriptionId, int toId)
        {
            return DoGet<SnapshotsResponse>("api/snapshots?subId=" + subscriptionId);
        }

        #endregion

        async Task<T> DoGet<T>(string url)
        {
            var r = await GetApiClient().client.GetAsync(url);
            if (r.StatusCode == HttpStatusCode.Forbidden)
                throw new NotAuthException();

            using (var s = await r.Content.ReadAsStreamAsync())
                return (T)new JsonSerializer().Deserialize(new StreamReader(s), typeof(T));
        }

        HttpClientHolder GetApiClient()
        {
            var c = GetCookieContainer();
            var client = new HttpClient(new HttpClientHandler
                {
                    CookieContainer = c,
                    UseCookies = true,
                }) { BaseAddress = BaseApi };
            return new HttpClientHolder { client = client, cookies = c };
        }

        CookieContainer GetCookieContainer()
        {
            var c = new CookieContainer();
            foreach (var s in authStorage.Load ())
                c.Add(BaseApi, new Cookie(s.Key, s.Value));
            return c;
        }

        class HttpClientHolder
        {
            internal HttpClient client;
            internal CookieContainer cookies;
        }
    }
}