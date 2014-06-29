using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using Spectator.Core.Model.Exceptions;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using Spectator.Core.Model;

namespace Spectator.Core.ViewModels
{
    public class MainViewModel : SpectatorViewModel
    {
        public ObservableCollection<object> Subscriptions { get; set; }
        private bool _isBusy;
        public bool IsBusy { get { return _isBusy; } set { Set(ref _isBusy, value); } }

        public bool _isNotRegistered;
        public bool IsNotAuthorized { get { return _isNotRegistered; } set { Set(ref _isNotRegistered, value); } }

        private ISubscriptionModel model = Mvx.Resolve<ISubscriptionModel>();

        public MainViewModel()
        {
            Subscriptions = new ObservableCollection<object>();
            Initialize();
        }

        private async void Initialize()
        {
            Subscriptions.Clear();
            IsBusy = true;
            IsNotAuthorized = false;
            try {
                var items = await model.GetSubscriptionsAsync();
                foreach (var s in items) Subscriptions.Add(s);
            } catch (WrongAuthException) {
                //ShowViewModel<LoginViewModel>(); // Не переходим, а показываем сообщение о неоходимости
                IsNotAuthorized = true;
            } catch (Exception) {
                //
            }
            IsBusy = false;
        }
    }
}
