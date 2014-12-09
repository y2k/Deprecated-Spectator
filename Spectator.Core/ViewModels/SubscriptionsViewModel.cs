using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.ViewModels.Messages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Spectator.Core.ViewModels
{
    public class SubscriptionsViewModel : ViewModelBase
    {
        public ObservableCollection<SubscriptionItemViewModel> Subscriptions { get; } = new ObservableCollection<SubscriptionItemViewModel>();

        public SubscriptionsViewModel()
        {
            ReloadSubscriptions();
        }

        async void ReloadSubscriptions()
        {
            var model = new SubscriptionCollectionModel();
            await model.Reload();
            Subscriptions.ReplaceAll((await model.Get()).Select(s => new SubscriptionItemViewModel(s)));
        }

        public class SubscriptionItemViewModel : ViewModelBase
        {
            public string Title { get; set; }

            public RelayCommand SelectCommand { get; set; }

            public SubscriptionItemViewModel(Subscription subscription)
            {
                Title = subscription?.Title;
                SelectCommand = new SpectatorRelayCommand(() =>
                    MessengerInstance.Send(new SelectSubscriptionMessage() { Id = subscription.Id }));
            }
        }
    }
}