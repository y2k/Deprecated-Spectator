using System;
using System.Collections.ObjectModel;
using Spectator.Core.Model.Database;
using Spectator.Core.ViewModels;
using Spectator.iOS.Common;
using Spectator.iOS.Platform;
using UIKit;

namespace Spectator.iOS
{
    public partial class MenuViewController : BaseUIViewController
    {
        public MenuViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var viewmodel = Scope.New<SubscriptionsViewModel>();

            SubscriptionList.DataSource = new SubscriptionDataSource(viewmodel.Subscriptions);
            viewmodel.Subscriptions.CollectionChanged += (sender, e) => SubscriptionList.ReloadData();

            LogoutButton.TouchUpInside += (sender, e) => CloseController();
            LogoutButton.SetCommand(viewmodel.LogoutCommand);

            SubscriptionList.Delegate = new Delegate(viewmodel);

            Scope.EndScope();
        }

        void CloseController()
        {
            DismissViewController(true, null);
        }

        public class SubscriptionDataSource : UITableViewDataSource
        {
            ObservableCollection<Subscription> subscriptions;

            public SubscriptionDataSource(ObservableCollection<Subscription> subscriptions)
            {
                this.subscriptions = subscriptions;
            }

            public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell("Subscription");
                var item = subscriptions[indexPath.Row];

                ((UILabel)cell.ViewWithTag(2)).Text = item.Title;

                var imageView = cell.ViewWithTag(1);
                imageView.Layer.CornerRadius = 20;

                if (item.ThumbnailImageId > 0)
                {
                    new ImageRequest()
                        .SetImageSize(50 * UIScreen.MainScreen.Scale)
                        .SetUri("" + item.ThumbnailImageId)
                        .To(imageView);
                }

                return cell;
            }

            public override nint RowsInSection(UITableView tableView, nint section)
            {
                return subscriptions.Count;
            }
        }

        public class Delegate : UITableViewDelegate
        {
            SubscriptionsViewModel viewmodel;

            public Delegate(SubscriptionsViewModel viewmodel)
            {
                this.viewmodel = viewmodel;
            }

            public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                viewmodel.SelectedItem = indexPath.Row;
            }
        }
    }
}