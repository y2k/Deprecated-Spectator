using System;
using Spectator.Core.ViewModels;
using Spectator.iOS.Common;
using UIKit;

namespace Spectator.iOS
{
    public partial class WebPreviewViewController : BaseUIViewController
    {
        WebPreviewViewModel viewmodel;

        public WebPreviewViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            viewmodel = Scope.New<WebPreviewViewModel>();

            var right = new UIBarButtonItem { Image = UIImage.FromBundle("ic_chrome_reader_mode_white.png") };
            right.Clicked += (sender, e) => ShowModeSelectMenu(right);
            NavigationItem.RightBarButtonItem = right;

            WebView.SetBinding((s, v) => s.LoadUrl(v), () => viewmodel.Url);

            Scope.EndScope();
        }

        void ShowModeSelectMenu(UIBarButtonItem right)
        {
            var action = new UIActionSheet("Show mode")
            {
                "Web preview".Translate(),
                "Difference".Translate(),
            };
            action.Clicked += (sender2, e2) =>
            {
                viewmodel.CurrentMode = e2.ButtonIndex == 0 
                        ? WebPreviewViewModel.Mode.WebPreview 
                        : WebPreviewViewModel.Mode.Difference;
            };
            action.ShowFrom(right, true);
        }
    }
}