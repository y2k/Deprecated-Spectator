using System;
using System.Collections.ObjectModel;
using Spectator.Core.Model.Database;
using Spectator.Core.ViewModels;
using Spectator.Core.ViewModels.Common;
using Spectator.iOS.Common;
using Spectator.iOS.Platform;
using UIKit;

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

            var action = new UIBarButtonItem(UIBarButtonSystemItem.Add);
            action.Clicked += (sender, e) => AddSubscription(action);
            NavigationItem.RightBarButtonItem = action;

            viewmodel = Scope.New<SnapshotsViewModel>();

            LoginButton.SetBinding((s, v) => s.Hidden = !v, () => viewmodel.IsAuthError);
            LoginButton.SetCommand(viewmodel.LoginCommand);

            List.DataSource = new SnapshotDataSource { Snapshots = viewmodel.Snapshots };
            List.Delegate = new SnapshotDelegate { Snapshots = viewmodel.Snapshots };
            viewmodel.Snapshots.CollectionChanged += (sender, e) => List.ReloadData();

            Scope.EndScope();
        }

        void AddSubscription(UIBarButtonItem action)
        {
            new CommandUIActionSheet()
                .AddCommand("Add site".Translate(), viewmodel.CreateSubscriptionCommand)
                .AddCommand("Create From RSS".Translate(), viewmodel.CreateFromRssCommand)
                .AddCancelButton("Cancel")
                .ShowFrom(action, true);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            MessengerInstance.Register<NavigationMessage>(
                this, typeof(LoginViewModel), _ => this.PushViewController("Login"));
            MessengerInstance.Register<SubscriptionsViewModel.NavigateToHome>(
                this, _ => this.ReplaceViewController("Main"));
            MessengerInstance.Register<SnapshotsViewModel.NavigateToSnapshotDetails>(
                this, message => this.PushViewController("Snapshot", message));
            MessengerInstance.Register<NavigationMessage>(
                this, typeof(ExtractRssViewModel), msg => this.PresentViewController("CreateFromRss", msg));
            MessengerInstance.Register<NavigationMessage>(this,
                typeof(CreateSubscriptionViewModel),
                msg => this.PresentViewController("CreateSubscription", msg));
            MessengerInstance.Register<ExtractRssViewModel.NavigateToCreateSubscription>(
                this, async msg =>
                {
                    await DismissViewControllerAsync(true);
                    this.PresentViewController("CreateSubscription", msg);
                });

            sideMenu.Activate();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            sideMenu.Deactive();
        }

        class SnapshotDataSource : UITableViewDataSource
        {
            internal ObservableCollection<Snapshot> Snapshots;

            public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell("Snapshot", indexPath);
                var item = Snapshots[indexPath.Row];
                
                ((UILabel)cell.ViewWithTag(2)).Text = item.Title;
                
                if (item.ThumbnailImageId > 0)
                {
                    new ImageRequest()
                        .SetImageSize(tableView.Frame.Width, tableView.Frame.Width / item.ThumbnailAspect)
                        .SetUri("" + item.ThumbnailImageId)
                        .To(cell.ViewWithTag(1));
                }

                return cell;
            }

            public override nint RowsInSection(UITableView tableView, nint section)
            {
                return Snapshots.Count;
            }
        }

        class SnapshotDelegate : UITableViewDelegate
        {
            internal ObservableCollection<Snapshot> Snapshots;

            public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                var item = Snapshots[indexPath.Row];
                if (item.ThumbnailHeight == 0)
                    return tableView.Frame.Width;
                return tableView.Frame.Width / item.ThumbnailAspect;
            }
        }
    }
}