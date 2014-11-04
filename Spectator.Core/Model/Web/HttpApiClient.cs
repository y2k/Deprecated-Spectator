﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Spectator.Core.Model.Exceptions;
using Spectator.Core.Model.Web.Proto;

namespace Spectator.Core.Model.Web
{
	public class HttpApiClient : IApiClient
	{
		readonly IAuthStorage authStorage = ServiceLocator.Current.GetInstance<IAuthStorage> ();

		#region IApiClient implementation

		public SubscriptionResponse GetSubscriptions ()
		{
			return DoGet<SubscriptionResponse> ("api/subscription");
		}

		public void LoginByCode (string code)
		{
			var web = GetClient ();
			var form = new [] { new KeyValuePair<string,string> ("code", code) };
			web.client.PostAsync ("Account/LoginByCode", new FormUrlEncodedContent (form)).Wait ();
			SaveCookieToAuthStorage (web.cookies);
		}

		void SaveCookieToAuthStorage (CookieContainer cookies)
		{
			authStorage.Save (cookies
				.GetCookies (Constants.BaseApi)
				.Cast<Cookie> ()
				.ToDictionary (s => s.Name, s => s.Value));
		}

		public SnapshotsResponse.ProtoSnapshot GetSnapshot (int serverId)
		{
			return DoGet<SnapshotsResponse.ProtoSnapshot> ("api/snapshot/" + serverId);
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

		[Obsolete]
		public T Get<T> (string url)
		{
			return DoGet<T> (url);
		}

		#endregion

		T DoGet<T> (string url)
		{
			var r = GetClient ().client.GetAsync (url).Result;
			if (r.StatusCode == HttpStatusCode.Forbidden)
				throw new NotAuthException ();

			using (var s = r.Content.ReadAsStreamAsync ().Result)
				return (T)new JsonSerializer ().Deserialize (new StreamReader (s), typeof(T));
		}

		HttpClientHolder GetClient ()
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