using System.Collections.ObjectModel;
using System.Windows.Input;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.ViewModels.Common;
using Spectator.Core.ViewModels.Messages;

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

        public ICommand LoginCommand { get; set; }

        public ICommand ReloadCommand { get; set; }

        public ICommand LoadMoreCommand { get; set; }

        public ICommand DeleteCurrentSubscriptionCommand { get; set; }

        public ICommand OpenSnapshotCommand { get; set; }

        public ICommand CreateFromRssCommand { get; set; }

        public ICommand CreateSubscriptionCommand { get; set; }

        public SnapshotsViewModel()
        {
            ChangeSubscriptionId(0);
            MessengerInstance.Register<SelectSubscriptionMessage>(this, s => ChangeSubscriptionId(s.Id));
            LoginCommand = new Command(
                () => MessengerInstance.Send(new NavigationMessage(), typeof(LoginViewModel)));
            DeleteCurrentSubscriptionCommand = new Command(
                () => new SubscriptionModel().Delete(GetCurrentSubscriptionId()));
            ReloadCommand = new Command(
                () => ChangeSubscriptionId(GetCurrentSubscriptionId()));
            CreateFromRssCommand = new Command(
                () => MessengerInstance.Send(new NavigationMessage(), typeof(ExtractRssViewModel)));
            CreateSubscriptionCommand = new Command(
                () => MessengerInstance.Send(new NavigationMessage(), typeof(CreateSubscriptionViewModel)));

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
    }
}