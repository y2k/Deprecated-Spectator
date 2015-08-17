using System;
using Spectator.iOS.Common;
using Spectator.Core.ViewModels;
using UIKit;
using System.Collections.ObjectModel;

namespace Spectator.iOS
{
    public partial class CreateFromRssViewController : BaseUIViewController
    {
        public CreateFromRssViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var viewmodel = Scope.New<ExtractRssViewModel>();

            CancelButton.Clicked += (sender, e) => DismissViewController(true, null);
            DoneButton.SetCommand(viewmodel.CeateCommand);
            ExtractButton.SetCommand(viewmodel.ExtractCommand);

            viewmodel.RssItems.CollectionChanged += (sender, e) => RssList.ReloadData();

            LinkText
                .SetBinding((s, v) => s.Text = v, () => viewmodel.Link)
                .SetTwoWay();            
            
            RssList.DataSource = new RssDataSource(viewmodel.RssItems);

            ActivitityIndicator.SetBinding((s, v) => s.Hidden = !v, () => viewmodel.IsBusy);

            Scope.EndScope();
        }

        class RssDataSource : UITableViewDataSource
        {
            ObservableCollection<ExtractRssViewModel.RssItemViewModel> items;

            internal RssDataSource(ObservableCollection<ExtractRssViewModel.RssItemViewModel> items)
            {
                this.items = items;
            }

            public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell("Rss");
                var i = items[indexPath.Row];
                cell.TextLabel.Text = i.Title;
                cell.DetailTextLabel.Text = i.Link;
                return cell;
            }

            public override nint RowsInSection(UITableView tableView, nint section)
            {
                return items.Count;
            }
        }
    }
}