using System;
using System.Collections.ObjectModel;
using CoreGraphics;
using Spectator.Core.Model.Database;
using Spectator.Core.ViewModels;
using Spectator.iOS.Common;
using Spectator.iOS.Platform;
using UIKit;

namespace Spectator.iOS
{
    public partial class MainViewController : BaseUIViewController
    {
        SideMenu sideMenu;

        public MainViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            sideMenu = new SideMenu(this, "Menu");
            sideMenu.Attach();
            SetCollectionLayout();

            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Action);

            var viewmodel = Scope.New<SnapshotsViewModel>();

            LoginButton.SetBinding((s, v) => s.Hidden = !v, () => viewmodel.IsAuthError);
            LoginButton.SetCommand(viewmodel.LoginCommand);

            SnapshotList.DataSource = new SnapshotDataSource(viewmodel.Snapshots);
            viewmodel.Snapshots.CollectionChanged += (sender, e) => SnapshotList.ReloadData();

            Scope.EndScope();
        }

        void SetCollectionLayout()
        {
            SnapshotList.CollectionViewLayout = new UICollectionViewFlowLayout
            {
                MinimumInteritemSpacing = 0,
                ItemSize = new CGSize(View.Frame.Width / 2, View.Frame.Width / 2 + 50),
            };
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            MessengerInstance.Register<SnapshotsViewModel.NavigateToLoginMessage>(
                this, _ => NavigationController.PushViewController(Storyboard.InstantiateViewController("Login"), true));
            MessengerInstance.Register<SubscriptionsViewModel.NavigateToHome>(
                this, _ => NavigationController.SetViewControllers(
                    new [] { Storyboard.InstantiateViewController("Main") }, true));

            sideMenu.Activate();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            sideMenu.Deactive();
        }

        public class SnapshotDataSource : UICollectionViewDataSource
        {
            ObservableCollection<Snapshot> snapshots;

            public SnapshotDataSource(ObservableCollection<Snapshot> snapshots)
            {
                this.snapshots = snapshots;
            }

            public override UICollectionViewCell GetCell(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
            {
                var cell = collectionView.DequeueReusableCell("Snapshot", indexPath);
                var item = snapshots[indexPath.Row];

                ((UILabel)cell.ViewWithTag(2)).Text = item.Title;
                new ImageRequest()
                    .SetUri("" + item.ThumbnailImageId)
                    .SetImageSize((int)(300 * UIScreen.MainScreen.Scale))
                    .To(cell.ViewWithTag(1));

                return (UICollectionViewCell)cell;
            }

            public override nint GetItemsCount(UICollectionView collectionView, nint section)
            {
                return snapshots.Count;
            }
        }
    }
}