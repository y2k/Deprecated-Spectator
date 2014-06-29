using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Uri LoginStartUrl
        {
            get { return LoginUrl; }
        }
    }
}
