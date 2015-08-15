using System;
using System.Collections.ObjectModel;
using Spectator.Core.Model.Database;
using Spectator.Core.ViewModels;
using Spectator.iOS.Common;
using UIKit;
using Spectator.iOS.Platform;

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

            CloseButton.TouchUpInside += (sender, e) => DismissViewController(true, null);

            var viewmodel = Scope.New<SubscriptionsViewModel>();

            SubscriptionList.DataSource = new SubscriptionDataSource(viewmodel.Subscriptions);
            viewmodel.Subscriptions.CollectionChanged += (sender, e) => SubscriptionList.ReloadData();

            Scope.EndScope();
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
                new ImageRequest()
                    .SetUri("" + item.ThumbnailImageId)
                    .SetImageSize((int)(50 * UIScreen.MainScreen.Scale))
                    .To(cell.ViewWithTag(1));

                return cell;
            }

            public override nint RowsInSection(UITableView tableView, nint section)
            {
                return subscriptions.Count;
            }
        }
    }
}