using System;
using System.Net.Http;
using System.Net;
using Spectator.Core.Model.Exceptions;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Spectator.Core.Model.Web.Proto;

namespace Spectator.Core.Model.Web
{
	class HttpApiClient : IApiClient
	{
		static readonly Lazy<HttpClientHolder> web = new Lazy<HttpClientHolder> (() => {
			var c = PersistenCookieContainer.LoadFromFileOrCreateEmpty (Constants.BaseApi);
			return new HttpClientHolder {
				cookies = c,
				client = new HttpClient (new HttpClientHandler {
					CookieContainer = c.Cookies,
					UseCookies = true,
				}) { BaseAddress = Constants.BaseApi },
			};
		});

		#region IApiClient implementation

		public SnapshotsResponse.ProtoSnapshot GetSnapshot (int serverId)
		{
			var url = "api/snapshot/" + serverId;
			return DoGet<SnapshotsResponse.ProtoSnapshot> (url);
		}

		public SnapshotsResponse Get (int toId)
		{
			var url = "api/snapshot";
			if (toId > 0)
				url = url + "?toId=" + toId;
			return DoGet<SnapshotsResponse> (url);
		}

		public SnapshotsResponse Get (int subscriptionId, int toId)
		{
			return DoGet<SnapshotsResponse> ("api/snapshot?subId=" + subscriptionId);
		}

		public void PostWebForm (string url, params object[] formKeyValues)
		{
			var c = new List<KeyValuePair<string, string>> ();
			for (int i = 0; i < formKeyValues.Length; i += 2)
				c.Add (new KeyValuePair<string, string> ("" + formKeyValues [i], "" + formKeyValues [i + 1]));

			web.Value.client.PostAsync (url, new FormUrlEncodedContent (c)).Wait ();
			web.Value.cookies.FlushToDiskAsync (url);
		}

		[Obsolete]
		public T Get<T> (string url)
		{
			return DoGet<T> (url);
		}

		#endregion

		static T DoGet<T> (string url)
		{
			var r = web.Value.client.GetAsync (url).Result;
			if (r.StatusCode == HttpStatusCode.Forbidden)
				throw new WrongAuthException ();
			try {
				using (var s = r.Content.ReadAsStreamAsync ().Result) {
					return (T)new JsonSerializer ().Deserialize (new StreamReader (s), typeof(T));
				}
			} finally {
				web.Value.cookies.FlushToDiskAsync (url);
			}
		}

		class HttpClientHolder
		{
			internal HttpClient client;
			internal PersistenCookieContainer cookies;
		}
	}
}