using GalaSoft.MvvmLight;
using Spectator.Core.Model.Account;

namespace Spectator.Core.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        GoogleUrlParser authUrlParser = new GoogleUrlParser();
        Account account = new Account();

        string _browserUrl;

        public string BrowserUrl
        {
            get { return _browserUrl; }
            set
            {
                Set(ref _browserUrl, value);
                BrowserUrlChanged();
            }
        }

        bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        public LoginViewModel()
        {
            _browserUrl = "" + authUrlParser.LoginStartUrl;
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "BrowserUrl")
                {
                    BrowserUrl.ToString();
                }
            };
        }

        void BrowserUrlChanged()
        {
            if (authUrlParser.IsStateSuccess(BrowserUrl))
                Login();
            else if (authUrlParser.IsStateAccessDenied(BrowserUrl))
                NavigateToHome();
        }

        async void Login()
        {
            IsBusy = true;
            await account.LoginByCode(authUrlParser.GetCode(BrowserUrl));
            NavigateToHome();
        }

        void NavigateToHome()
        {
            MessengerInstance.Send(new NavigateHomeMessage());
        }

        public class NavigateHomeMessage
        {
        }
    }
}