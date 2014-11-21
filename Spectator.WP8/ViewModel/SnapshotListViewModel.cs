using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.WP8.ViewModel.Base;
using Spectator.WP8.ViewModel.Common;
using System.Collections.ObjectModel;
using System.Linq;

namespace Spectator.WP8.ViewModel
{
    public class SnapshotListViewModel : BaseViewModel
    {
        public ObservableCollection<SnapshotItemViewModel> Snapshots { get; } = new ObservableCollection<SnapshotItemViewModel>();

        public async void ChangeSubscriptionId(int subscriptionId)
        {
            Snapshots.Clear();
            var model = new SnapshotCollectionModel(subscriptionId);
            await model.Reset();
            await model.Next();
            Snapshots.ReplaceAll((await model.Get()).Select(s => new SnapshotItemViewModel(s)));
        }

        public class SnapshotItemViewModel : BaseViewModel
        {
            ImageModel model = ServiceLocator.Current.GetInstance<ImageModel>();
            Snapshot snapshot;

            public string Title { get { return snapshot.Title; } }

            public string Thumbnail { get { return GetThumbnail(400); } }

            private string GetThumbnail(int maxWidth)
            {
                return model.GetThumbnailUrl(snapshot.ThumbnailImageId, maxWidth);
            }

            public SnapshotItemViewModel(Snapshot snapshot)
            {
                this.snapshot = snapshot;
            }
        }
    }
}