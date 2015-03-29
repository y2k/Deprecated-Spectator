using System;
using System.Text.RegularExpressions;

namespace Spectator.Core.Model.Account
{
	public class GoogleUrlParser
	{
        public const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob:auto";

		static readonly Regex CodeRegex = new Regex ("code=(.+)");
		static readonly Regex AccessDeniedRegex = new Regex ("http://localhost/\\?error=access_denied");
		static readonly Uri LoginUrl = new Uri ("https://accounts.google.com/o/oauth2/auth?"
		                               + "response_type=code"
		                               + "&client_id=445037560545.apps.googleusercontent.com"
		                               + "&scope=" + Uri.EscapeDataString ("https://www.googleapis.com/auth/userinfo.email")
		                               + "&redirect_uri=" + Uri.EscapeDataString (RedirectUri)
		                               + "&access_type=offline");

		public Uri LoginStartUrl {
			get { return LoginUrl; }
		}

		public bool IsStateAccessDenied (string url)
		{
			return AccessDeniedRegex.IsMatch (Normalize (url));
		}

		public bool IsStateSuccess (string url)
		{
			return CodeRegex.IsMatch (Normalize (url));
		}

		public string GetCode (string url)
		{
			return CodeRegex.Match (Normalize (url)).Groups [1].Value;
		}

		static string Normalize (string url)
		{
			return url ?? "";
		}
	}
}