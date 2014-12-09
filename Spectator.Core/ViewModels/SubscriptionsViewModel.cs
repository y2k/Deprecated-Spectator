using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using System.Collections.ObjectModel;
using System.Linq;

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
            public RelayCommand SelectCommand { get; set; }

            Subscription subscription;

            public string Title
            {
                get { return subscription?.Title; }
            }

            public SubscriptionItemViewModel(Subscription subscription)
            {
                this.subscription = subscription;
                SelectCommand = new RelayCommand(() =>
                {
                    // TODO:
                    //hostViewModel.SnapshotsViewModel.ChangeSubscriptionId(subscription.Id);
                });
            }
        }
    }
}