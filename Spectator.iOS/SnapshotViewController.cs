using System;
using Spectator.Core.ViewModels;
using Spectator.iOS.Common;

namespace Spectator.iOS
{
    public partial class SnapshotViewController : BaseUIViewController
    {
        public SnapshotViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var viewmodel = Scope.New<SnapshotViewModel>();

            WebView.SetBinding((s, v) => s.LoadUrl(v), () => viewmodel.PreviewUrl);

            WebButton.SetCommand(viewmodel.SetModeWebCommand);
            DiffButton.SetCommand(viewmodel.SetModeDiffCommand);

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