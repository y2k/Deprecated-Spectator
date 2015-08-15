using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.ViewModels.Messages;
using System.Collections.ObjectModel;

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

        void SelectedSubscriptionChanged()
        {
            MessengerInstance.Send(new SelectSubscriptionMessage() { Id = Subscriptions[_selectedItem].Id });
        }

        public SubscriptionsViewModel()
        {
            ReloadSubscriptions();
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
    }
}