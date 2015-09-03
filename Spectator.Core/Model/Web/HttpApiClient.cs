using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;

namespace Spectator.Core.Model.Web
{
    public class HttpApiClient : ISpectatorApi
    {
        static readonly Uri BaseApi = new Uri("http://spectator-api.cloudapp.net/");
        readonly IAuthProvider authStorage = ServiceLocator.Current.GetInstance<IAuthProvider>();

        #region IApiClient implementation

        public Uri CreateFullUrl(string relativePath)
        {
            return new Uri(BaseApi, relativePath);
        }

        public async Task SendPushToken(string userToken, int platformId)
        {
            var form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("RegistrationId", userToken),
                    new KeyValuePair<string,string>("PlatformId", "" + platformId),
                });
            var web = await GetApiClient();
            await web.client.PostAsync("api/push/", form);
        }

        public async Task EditSubscription(int id, string title)
        {
            var form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("Title", title),
                });
            var web = await GetApiClient();
            await web.client.PostAsync("api/subscriptions/" + id, form);
        }

        public async Task DeleteSubscription(int id)
        {
            await (await GetApiClient()).client.DeleteAsync("api/subscriptions/" + id);
        }

        public async Task CreateSubscription(Uri link, string title)
        {
            var form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("Source", link.AbsoluteUri),
                    new KeyValuePair<string,string>("Title", title),
                });
            var web = await GetApiClient();
            await web.client.PutAsync("api/subscription", form);
        }

        public Task<SubscriptionResponse> GetSubscriptions()
        {
            return DoGet<SubscriptionResponse>("api/subscriptions");
        }

        public async Task<IDictionary<string, string>> LoginByCode(string code, string redirectUri)
        {
            var web = await GetApiClient();
            var form = new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirectUri", redirectUri),
            };
            await web.client.PostAsync("Account/LoginByCode", new FormUrlEncodedContent(form));
            return CookiesToDictionary(web.cookies);
        }

        IDictionary<string, string> CookiesToDictionary(CookieContainer cookies)
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
            var r = await (await GetApiClient()).client.GetAsync(url);
            if (r.StatusCode == HttpStatusCode.Forbidden)
                throw new NotAuthException();

            using (var s = await r.Content.ReadAsStreamAsync())
                return (T)new JsonSerializer().Deserialize(new StreamReader(s), typeof(T));
        }

        async Task<HttpClientHolder> GetApiClient()
        {
            var cookies = await GetCookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookies,
                UseCookies = true,
            };
            var client = new HttpClient(handler) { BaseAddress = BaseApi };
            return new HttpClientHolder { client = client, cookies = cookies };
        }

        async Task<CookieContainer> GetCookieContainer()
        {
            var c = new CookieContainer();
            foreach (var s in await authStorage.Load())
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