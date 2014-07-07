using System;
using System.Linq;
using System.Net.Http;
using Spectator.Core.Model.Web.Proto;
using ProtoBuf;
using System.Net;
using Spectator.Core.Model.Exceptions;
using System.Collections.Generic;
using PCLStorage;
using System.Threading.Tasks;
using System.Text;

namespace Spectator.Core.Model.Web
{
	internal class WebConnect : IWebConnect
	{
		private static readonly Lazy<HttpClientHolder> web = new Lazy<HttpClientHolder> (() => {
			var c = PersistenCookieContainer.LoadFromFileOrCreateEmpty ();
			return new HttpClientHolder {
				cookies = c,
				client = new HttpClient (new HttpClientHandler {
					CookieContainer = c.Cookies,
					UseCookies = true,
				}) { BaseAddress = Constants.BaseApi },
			};
		});

		#region IWebConnect implementation

		public void PostWebForm (string url, params object[] formKeyValues)
		{
			var c = new List<KeyValuePair<string, string>> ();
			for (int i = 0; i < formKeyValues.Length; i += 2) {
				c.Add (new KeyValuePair<string, string> ("" + formKeyValues [i], "" + formKeyValues [i + 1]));
			}

			web.Value.client.PostAsync (url, new FormUrlEncodedContent (c)).Wait ();
			web.Value.cookies.FlushToDiskAsync (url);
		}

		public T Get<T> (string url)
		{
			var r = web.Value.client.GetAsync (url).Result;
			if (r.StatusCode == HttpStatusCode.Forbidden)
				throw new WrongAuthException ();

			try {
				using (var s = r.Content.ReadAsStreamAsync ().Result) {
					return Serializer.Deserialize<T> (s);
				}
			} finally {
				web.Value.cookies.FlushToDiskAsync (url);
			}
		}

		#endregion

		private class HttpClientHolder
		{
			internal HttpClient client;
			internal PersistenCookieContainer cookies;
		}
	}
}