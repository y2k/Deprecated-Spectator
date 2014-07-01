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

		public void LoadSnapshots (int id)
		{
			var r = client.Value.GetAsync ("http://debug.spectator.api-i-twister.net/api/snapshot2").Result;
			if (r.StatusCode == HttpStatusCode.Forbidden)
				throw new WrongAuthException ();

			using (var s = client.Value.GetStreamAsync ("http://debug.spectator.api-i-twister.net/api/snapshot2").Result) {
				var z = Serializer.Deserialize<ProtoSnapshotsResponse> (s);
				z.ToString ();
			}

			throw new Exception ();
		}

		public void PostWebForm (string url, params object[] formKeyValues)
		{
			var c = new List<KeyValuePair<string, string>> ();
			for (int i = 0; i < formKeyValues.Length; i += 2) {
				c.Add (new KeyValuePair<string, string> ("" + formKeyValues [i], "" + formKeyValues [i + 1]));
			}

			client.Value.PostAsync (url, new FormUrlEncodedContent (c)).Wait ();
		}

		#endregion
	}
}