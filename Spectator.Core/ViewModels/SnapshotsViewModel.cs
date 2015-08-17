using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.ViewModels.Messages;
using Spectator.Core.ViewModels.Common;
using System.Windows.Input;

namespace Spectator.Core.ViewModels
{
    public class SnapshotsViewModel : ViewModel
    {
        public ObservableCollection<Snapshot> Snapshots { get; } = new ObservableCollection<Snapshot>();

        bool _isAuthError;

        public bool IsAuthError
        {
            get { return _isAuthError; }
            set { Set(ref _isAuthError, value); }
        }

        public bool IsGeneralError
        {
            get { return false; }
            set { /* TODO */ }
        }

        bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        public RelayCommand LoginCommand { get; set; }

        public RelayCommand ReloadCommand { get; set; }

        public RelayCommand LoadMoreCommand { get; set; }

        public RelayCommand DeleteCurrentSubscriptionCommand { get; set; }

        public RelayCommand CreateSubscriptionCommand { get; set; }

        public ICommand OpenSnapshotCommand { get; set; }

        public ICommand CreateFromRssCommand { get; set; }

        public SnapshotsViewModel()
        {
            ChangeSubscriptionId(0);
            MessengerInstance.Register<SelectSubscriptionMessage>(this, s => ChangeSubscriptionId(s.Id));
            LoginCommand = new Command(
                () => MessengerInstance.Send(new NavigateToLoginMessage()));
            CreateSubscriptionCommand = new Command(
                () => MessengerInstance.Send(new NavigateToCreateSubscriptionMessage()));
            DeleteCurrentSubscriptionCommand = new Command(
                () => new SubscriptionModel().Delete(GetCurrentSubscriptionId()));
            ReloadCommand = new Command(
                () => ChangeSubscriptionId(GetCurrentSubscriptionId()));
            CreateFromRssCommand = new Command(
                () => MessengerInstance.Send(new NavigateToCreateFromRss()));

            OpenSnapshotCommand = new Command<int>(
                position =>
                {
                    var msg = new NavigateToWebPreview{ SnashotId = Snapshots[position].Id };
                    MessengerInstance.Send(msg);
                });
        }

        int GetCurrentSubscriptionId()
        {
            return Snapshots[0].SubscriptionId;
        }

        public async void ChangeSubscriptionId(int subscriptionId)
        {
            IsBusy = true;
            Snapshots.Clear();
            var model = new SnapshotCollectionModel(subscriptionId);
            await model.Reset();
            try
            {
                await model.Next();
            }
            catch (NotAuthException)
            {
                IsAuthError = true;
            }
            IsBusy = false;
            Snapshots.ReplaceAll(await model.Get());
        }

        public class NavigateToWebPreview : NavigationMessage
        {
            public int SnashotId { get ; set ; }
        }

        public class NavigateToLoginMessage : NavigationMessage
        {
        }

        public class NavigateToCreateSubscriptionMessage : NavigationMessage
        {
        }

        public class NavigateToCreateFromRss : NavigationMessage
        {
        }
    }
}