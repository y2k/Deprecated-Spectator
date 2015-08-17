using Spectator.Core.Model.Account;
using Spectator.Core.ViewModels.Common;

namespace Spectator.Core.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        GoogleUrlParser authUrlParser = new GoogleUrlParser();
        Account account = new Account();

        string _browserUrl;
        public string BrowserUrl
        {
            get { return _browserUrl; }
            set { Set(ref _browserUrl, value); }
        }

        bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        string _browserTitle;
        public string BrowserTitle
        {
            get { return _browserTitle; }
            set { Set(ref _browserTitle, value); BrowserUrlChanged(); }
        }

        public LoginViewModel()
        {
            _browserUrl = "" + authUrlParser.LoginStartUrl;
        }

        void BrowserUrlChanged()
        {
            if (authUrlParser.IsStateSuccess(BrowserTitle)) Login();
            else if (authUrlParser.IsStateAccessDenied(BrowserTitle)) NavigateToHome();
        }

        async void Login()
        {
            IsBusy = true;
            await account.LoginByCode(authUrlParser.GetCode(BrowserTitle));
            NavigateToHome();
        }

        void NavigateToHome()
        {
            MessengerInstance.Send(new NavigateToHomeMessage());
        }

        public class NavigateToHomeMessage : NavigationMessage
        {
        }
    }
}