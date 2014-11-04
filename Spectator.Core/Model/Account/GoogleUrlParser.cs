﻿using System;
using System.Text.RegularExpressions;

namespace Spectator.Core.Model.Account
{
	class GoogleUrlParser
	{
		static readonly Regex CodeRegex = new Regex ("http://localhost/\\?code=(.+)");
		static readonly Regex AccessDeniedRegex = new Regex ("http://localhost/\\?error=access_denied");
		static readonly Uri LoginUrl = new Uri ("https://accounts.google.com/o/oauth2/auth?"
		                               + "response_type=code"
		                               + "&client_id=445037560545.apps.googleusercontent.com"
		                               + "&scope=" + Uri.EscapeDataString ("https://www.googleapis.com/auth/userinfo.email")
		                               + "&redirect_uri=" + Uri.EscapeDataString ("http://localhost")
		                               + "&access_type=offline");

		public Uri LoginStartUrl {
			get { return LoginUrl; }
		}

		public bool IsStateAccessDenied (string url)
		{
			return AccessDeniedRegex.IsMatch (url);
		}

		public bool IsStateSuccess (string url)
		{
			return CodeRegex.IsMatch (url);
		}

		public string GetToken (string url)
		{
			return CodeRegex.Match (url).Groups [1].Value;
		}
	}
}