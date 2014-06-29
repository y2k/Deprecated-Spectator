using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;

namespace Spectator.Core.Model
{
	class ProfileModel : IProfileModel
    {
        private static readonly Uri LoginUrl = new Uri("https://accounts.google.com/o/oauth2/auth?"
                     + "response_type=code"
                     + "&client_id=445037560545.apps.googleusercontent.com"
                     + "&scope=" + Uri.EscapeDataString("https://www.googleapis.com/auth/userinfo.email")
                     + "&redirect_uri=" + Uri.EscapeDataString("http://localhost")
                     + "&access_type=offline");

		private static readonly Regex CodeRegex = new Regex("http://localhost/\\?code=(.+)");
		private static readonly Regex AccessDeniedRegex = new Regex("http://localhost/\\?error=access_denied");

        public Uri LoginStartUrl
        {
            get { return LoginUrl; }
        }

		public bool IsAccessDenied (string url)
		{
			return AccessDeniedRegex.IsMatch (url);
		}

		public bool IsValid (string url)
		{
			return CodeRegex.IsMatch (url);
		}

		public Task LoginViaCodeAsync (string url)
		{
			return Task.Run (() => {
				new ManualResetEvent(false).WaitOne(2000);
			});
		}
    }
}