using GalaSoft.MvvmLight;
using Spectator.Core.Model.Account;
using System;

namespace Spectator.Core.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        GoogleUrlParser authUrlParser = new GoogleUrlParser();
        Account account = new Account();

        string _browserUrl;

        [Obsolete]
        public string BrowserUrl
        {
            get { return _browserUrl; }
            set { Set(ref _browserUrl, value); BrowserUrlChanged(); }
        }

        bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        private string _browserTitle;
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

        public class NavigateToHomeMessage
        {
        }
    }
}