using System.Collections.ObjectModel;
using System.Windows.Input;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.ViewModels.Messages;
using Spectator.Core.Model.Account;
using Spectator.Core.ViewModels.Common;

namespace Spectator.Core.ViewModels
{
    public class SubscriptionsViewModel : ViewModel
    {
        public ObservableCollection<Subscription> Subscriptions { get; } = new ObservableCollection<Subscription>();

        int _selectedItem;

        public int SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                Set(ref _selectedItem, value);
                SelectedSubscriptionChanged();
            }
        }

        public ICommand LogoutCommand { get; set; }

        public SubscriptionsViewModel()
        {
            ReloadSubscriptions();
            LogoutCommand = new Command(
                async () =>
                {
                    await new Account().Logout();
                    MessengerInstance.Send(new NavigateToHome());
                });
        }

        void SelectedSubscriptionChanged()
        {
            MessengerInstance.Send(new SelectSubscriptionMessage { Id = Subscriptions[_selectedItem].Id });
        }

        async void ReloadSubscriptions()
        {
            var model = new SubscriptionCollectionModel();
            try
            {
                await model.Reload();
            }
            catch (NotAuthException e)
            {
                // TODO:
                e.ToString();
            }
            Subscriptions.ReplaceAll(await model.Get());
        }

        public class NavigateToHome : INavigationMessage
        {
        }
    }
}