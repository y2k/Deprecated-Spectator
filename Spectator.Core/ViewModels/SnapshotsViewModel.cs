using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.ViewModels.Messages;
using System.Collections.ObjectModel;
using System.Linq;

namespace Spectator.Core.ViewModels
{
    public class SnapshotsViewModel : ViewModelBase
    {
        public ObservableCollection<SnapshotItemViewModel> Snapshots { get; } = new ObservableCollection<SnapshotItemViewModel>();

        bool _isAuthError;
        public bool IsAuthError
        {
            get { return _isAuthError; }
            set { Set(ref _isAuthError, value); }
        }

        public RelayCommand LoginCommand { get; set; }

        public SnapshotsViewModel()
        {
            ChangeSubscriptionId(0);
            MessengerInstance.Register<SelectSubscriptionMessage>(this, s => ChangeSubscriptionId(s.Id));
            LoginCommand = new SpectatorRelayCommand(() => MessengerInstance.Send(new NavigateToLoginMessage()));
        }

        public async void ChangeSubscriptionId(int subscriptionId)
        {
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
            Snapshots.ReplaceAll((await model.Get()).Select(s => new SnapshotItemViewModel(s)));
        }

        public class SnapshotItemViewModel : ViewModelBase
        {
            ImageModel model = ServiceLocator.Current.GetInstance<ImageModel>();
            Snapshot snapshot;

            public string Title { get { return snapshot.Title; } }

            public string Thumbnail { get { return GetThumbnail(400); } }

            private string GetThumbnail(int maxWidth)
            {
                return new ImageIdToUrlConverter().GetThumbnailUrl(snapshot.ThumbnailImageId, maxWidth);
            }

            public SnapshotItemViewModel(Snapshot snapshot)
            {
                this.snapshot = snapshot;
            }
        }

        public class NavigateToLoginMessage { }
    }
}
