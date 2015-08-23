using System;
using Spectator.Core.ViewModels;
using Spectator.iOS.Common;
using UIKit;
using System.Collections.ObjectModel;
using Spectator.Core.Model.Database;
using Spectator.iOS.Platform;

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

            TitleLabel.SetBinding((s, v) => s.Text = v ?? "…", () => viewmodel.Title);
            CreatedLabel.SetBinding((s, v) => s.Text = DateToString(v), () => viewmodel.Created);
            UrlLabel.SetBinding((s, v) => s.Text = v ?? "…", () => viewmodel.SourceUrl);

            List.DataSource = new DataSource { Attachments = viewmodel.Attachments };
            viewmodel.Attachments.CollectionChanged += (sender, e) => List.ReloadData();

            Scope.EndScope();
        }

        static string DateToString(DateTime date)
        {
            return date == DateTime.MinValue ? "…" : ("" + date);
        }

        class DataSource : UICollectionViewDataSource
        {
            public ObservableCollection<Attachment> Attachments { get; set; }

            public override UICollectionViewCell GetCell(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
            {
                var cell = collectionView.DequeueReusableCell("Attachment", indexPath);
                var item = Attachments[indexPath.Row];

                new ImageRequest()
                    .SetUri(item.Image)
                    .SetImageSize((int)(150 * UIScreen.MainScreen.Scale))
                    .To(cell.ViewWithTag(1));

                return (UICollectionViewCell)cell;
            }

            public override nint GetItemsCount(UICollectionView collectionView, nint section)
            {
                return Attachments.Count;
            }
        }
    }
}