using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Exceptions;
using Spectator.Core.Model.Tasks;
using Spectator.WP8.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Spectator.WP8.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Subscription> Subscriptions { get; private set; }

        private ISubscriptionModel model = ServiceLocator.Current.GetInstance<ISubscriptionModel>();

        public MainViewModel()
        {
            Subscriptions = new ObservableCollection<Subscription>();
        }

        public override void OnStart()
        {
            model.SubscriptionChanged += model_SubscriptionChanged;
            model.ReloadList();
        }

        public override void OnStop()
        {
            model.SubscriptionChanged -= model_SubscriptionChanged;
        }

        private void model_SubscriptionChanged(object sender, Result<IEnumerable<Subscription>> t)
        {
            Subscriptions.Clear();

            if (t.Value != null) foreach (var s in t.Value) Subscriptions.Add(s);
            else if (t.Error is WrongAuthException) NavigateToViewModel<ProfileViewModel>();
        }
    }
}