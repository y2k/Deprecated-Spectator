using Cirrious.CrossCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectator.Core.Model;

namespace Spectator.Core.ViewModels
{
    public class LoginViewModel : SpectatorViewModel
    {
        private IProfileModel model = Mvx.Resolve<IProfileModel>();

        public LoginViewModel()
        {
            //
        }

        public void Initialize(ICommonWebView webview)
        {
            webview.OpenUrl(model.LoginStartUrl);
        }

        public interface ICommonWebView
        {
            void OpenUrl(Uri url);
        }
    }
}