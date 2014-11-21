using GalaSoft.MvvmLight.Command;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.WP8.ViewModel.Base;
using Spectator.WP8.ViewModel.Common;
using System.Collections.ObjectModel;
using System.Linq;

namespace Spectator.WP8.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<SubscriptionItemViewModel> Subscriptions { get; } = new ObservableCollection<SubscriptionItemViewModel>();

        public SnapshotListViewModel SnapshotsViewModel { get; } = new SnapshotListViewModel();

        public RelayCommand AddSubscriptionCommand { get; set; }

        public MainViewModel()
        {
            AddSubscriptionCommand = new RelayCommand(() => NavigateToViewModel<CreateSubscriptionViewModel>());
        }

        public override void OnStart()
        {
            ReloadSubscriptions();
        }

        async void ReloadSubscriptions()
        {
            var model = new SubscriptionCollectionModel();
            await model.Reload();
            Subscriptions.ReplaceAll((await model.Get()).Select(s => new SubscriptionItemViewModel(s, this)));
        }

        public class SubscriptionItemViewModel : BaseViewModel
        {
            public RelayCommand SelectCommand { get; set; }

            Subscription subscription;

            public string Title
            {
                get { return subscription?.Title; }
            }

            public SubscriptionItemViewModel(Subscription subscription, MainViewModel hostViewModel)
            {
                this.subscription = subscription;
                SelectCommand = new RelayCommand(() =>
                      hostViewModel.SnapshotsViewModel.ChangeSubscriptionId(subscription.Id));
            }
        }
    }
}