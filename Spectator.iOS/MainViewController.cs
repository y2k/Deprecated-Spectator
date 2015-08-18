using System;
using System.Collections.ObjectModel;
using CoreGraphics;
using Spectator.Core.Model.Database;
using Spectator.Core.ViewModels;
using Spectator.iOS.Common;
using Spectator.iOS.Platform;
using UIKit;
using Spectator.Core.ViewModels.Common;

namespace Spectator.iOS
{
    public partial class MainViewController : BaseUIViewController
    {
        SideMenu sideMenu;

        SnapshotsViewModel viewmodel;

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

            var action = new UIBarButtonItem(UIBarButtonSystemItem.Add);
            action.Clicked += (sender, e) => AddSubscription(action);
            NavigationItem.RightBarButtonItem = action;

            viewmodel = Scope.New<SnapshotsViewModel>();

            LoginButton.SetBinding((s, v) => s.Hidden = !v, () => viewmodel.IsAuthError);
            LoginButton.SetCommand(viewmodel.LoginCommand);

            SnapshotList.DataSource = new SnapshotDataSource(viewmodel.Snapshots);
            SnapshotList.Delegate = new SnapshotDelegate(viewmodel);
            viewmodel.Snapshots.CollectionChanged += (sender, e) => SnapshotList.ReloadData();

            Scope.EndScope();
        }

        void AddSubscription(UIBarButtonItem action)
        {
            var sheet = new UIActionSheet
            {
                "Add site".Translate(),
                "Create From RSS".Translate(),
            };
            sheet.Clicked += (sender2, e2) =>
            {
                if (e2.ButtonIndex == 0)
                    viewmodel.CreateSubscriptionCommand.Execute(null);
                if (e2.ButtonIndex == 1)
                    viewmodel.CreateFromRssCommand.Execute(null);
            };
            sheet.CancelButtonIndex = sheet.AddButton("Cancel");
            sheet.ShowFrom(action, true);
        }

        void SetCollectionLayout()
        {
            SnapshotList.CollectionViewLayout = new UICollectionViewFlowLayout
            {
                MinimumInteritemSpacing = 0,
                ItemSize = new CGSize(View.Frame.Width / 2, View.Frame.Width / 2 + 50),
                FooterReferenceSize = new CGSize(50, 50),
            };
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            MessengerInstance.Register<SnapshotsViewModel.NavigateToLoginMessage>(
                this, _ => this.PushViewController("Login"));
            MessengerInstance.Register<SubscriptionsViewModel.NavigateToHome>(
                this, _ => this.ReplaceViewController("Main"));
            MessengerInstance.Register<SnapshotsViewModel.NavigateToWebPreview>(
                this, message => this.PushViewController("WebPreview", message));
            MessengerInstance.Register<SnapshotsViewModel.NavigateToCreateFromRss>(
                this, _ => this.PresentViewController("CreateFromRss"));
            MessengerInstance.Register<NavigationMessage>(this,
                typeof(CreateSubscriptionViewModel),
                msg => this.PresentViewController("CreateSubscription"));

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

                if (item.ThumbnailImageId > 0)
                {
                    new ImageRequest()
                        .SetUri("" + item.ThumbnailImageId)
                        .SetImageSize((int)(300 * UIScreen.MainScreen.Scale))
                        .To(cell.ViewWithTag(1));
                }

                return (UICollectionViewCell)cell;
            }

            public override nint GetItemsCount(UICollectionView collectionView, nint section)
            {
                return snapshots.Count;
            }

            public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, Foundation.NSString elementKind, Foundation.NSIndexPath indexPath)
            {
                UICollectionReusableView result = null;

                if (elementKind == UICollectionElementKindSectionKey.Footer)
                {
                    result = collectionView.DequeueReusableSupplementaryView(elementKind, "LoadMore", indexPath);
                }

                return result;
            }
        }

        public class SnapshotDelegate : UICollectionViewDelegate
        {
            SnapshotsViewModel viewmodel;

            public SnapshotDelegate(SnapshotsViewModel viewmodel)
            {
                this.viewmodel = viewmodel;
            }

            public override void ItemSelected(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
            {
                viewmodel.OpenSnapshotCommand.Execute(indexPath.Row);
            }
        }
    }
}