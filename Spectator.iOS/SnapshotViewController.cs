using System;
using Spectator.Core.ViewModels;
using Spectator.iOS.Common;

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

            Scope.EndScope();
        }

        static string DateToString(DateTime date)
        {
            return date == DateTime.MinValue ? "…" : ("" + date);
        }
    }
}