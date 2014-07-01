using System;
using System.Linq;
using System.Net.Http;
using Spectator.Core.Model.Web.Proto;
using ProtoBuf;
using System.Net;
using Spectator.Core.Model.Exceptions;
using System.Collections.Generic;

namespace Spectator.Core.Model.Web
{
	internal class WebConnect : IWebConnect
	{
		private static readonly Lazy<HttpClient> client = new Lazy<HttpClient> (() => {
			var h = new HttpClientHandler ();
			h.CookieContainer = new System.Net.CookieContainer ();
			h.UseCookies = true;

			var c = new HttpClient (h);
			return c;
		});

		#region IWebConnect implementation

		public void PostWebForm (string url, params object[] formKeyValues)
		{
			var c = new List<KeyValuePair<string, string>> ();
			for (int i = 0; i < formKeyValues.Length; i += 2) {
				c.Add (new KeyValuePair<string, string> ("" + formKeyValues [i], "" + formKeyValues [i + 1]));
			}

			client.Value.PostAsync (url, new FormUrlEncodedContent (c)).Wait ();
		}

		public T Get<T> (string url)
		{
			var r = client.Value.GetAsync (url).Result;
			if (r.StatusCode == HttpStatusCode.Forbidden)
				throw new WrongAuthException ();

			using (var s = r.Content.ReadAsStreamAsync().Result) {
				return Serializer.Deserialize<T> (s);
			}
		}

		#endregion
	}
}