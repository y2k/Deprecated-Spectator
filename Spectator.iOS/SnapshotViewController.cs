using System;
using Spectator.Core.Model.Database;
using Spectator.Core.ViewModels;
using Spectator.iOS.Common;
using Spectator.iOS.Platform;
using UIKit;

namespace Spectator.iOS
{
    public partial class SnapshotViewController : BaseUIViewController
    {
        SnapshotViewModel viewmodel;

        public SnapshotViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            WebButton.Clicked += (sender, e) =>
            {
                WebView.LoadUrl(viewmodel.ContentUrl);
                InformationPanel.Hidden = true;
            };
            DiffButton.Clicked += (sender, e) =>
            {
                WebView.LoadUrl(viewmodel.DiffUrl);
                InformationPanel.Hidden = true;
            };
            DetailsButton.Clicked += (sender, e) =>
            {
                WebView.LoadUrl(null);
                InformationPanel.Hidden = false;
            };
            WebView.ScalesPageToFit = true;

            viewmodel = Scope.New<SnapshotViewModel>();

            WebButton.SetBinding((s, v) => s.Enabled = v != null, () => viewmodel.ContentUrl);
            DiffButton.SetBinding((s, v) => s.Enabled = v != null, () => viewmodel.DiffUrl);

            List.SetBinding(List.ReloadData, () => viewmodel.Title);
            List.SetBinding(List.ReloadData, () => viewmodel.Created);
            List.SetBinding(List.ReloadData, () => viewmodel.SourceUrl);

            List.DataSource = new DataSource { viewmodel = viewmodel };
            viewmodel.Attachments.CollectionChanged += (sender, e) => List.ReloadData();

            Scope.EndScope();
        }

        static string DateToString(DateTime date)
        {
            return date == DateTime.MinValue ? "…" : ("" + date);
        }

        class DataSource : UICollectionViewDataSource
        {
            public SnapshotViewModel viewmodel { get; set; }

            public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, Foundation.NSString elementKind, Foundation.NSIndexPath indexPath)
            {
                var header = collectionView.DequeueReusableSupplementaryView(UICollectionElementKindSection.Header, "Header", indexPath);

                ((UILabel)header.ViewWithTag(1)).Text = viewmodel.Title;
                ((UILabel)header.ViewWithTag(2)).Text = "" + viewmodel.Created;
                ((UILabel)header.ViewWithTag(3)).Text = viewmodel.SourceUrl;

                return header;
            }

            public override UICollectionViewCell GetCell(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
            {
                var cell = collectionView.DequeueReusableCell("Attachment", indexPath);
                var item = viewmodel.Attachments[indexPath.Row];

                new ImageRequest()
                    .SetUri(item.Image)
                    .SetImageSize((int)(150 * UIScreen.MainScreen.Scale))
                    .To(cell.ViewWithTag(1));

                return (UICollectionViewCell)cell;
            }

            public override nint GetItemsCount(UICollectionView collectionView, nint section)
            {
                return viewmodel.Attachments.Count;
            }
        }
    }
}