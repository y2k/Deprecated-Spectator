using System;
using Spectator.Core.ViewModels;
using Spectator.iOS.Common;
using UIKit;

namespace Spectator.iOS
{
    public partial class MainViewController : BaseUIViewController
    {
        public MainViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var viewmodel = Scope.New<SnapshotsViewModel>();

            LoginButton.SetBinding((s, v) => s.Hidden = !v, () => viewmodel.IsAuthError);
            LoginButton.SetCommand(viewmodel.LoginCommand);

            var menuButton = new UIBarButtonItem{ Image = UIImage.FromBundle("ic_menu_white.png") };
            menuButton.Clicked += (sender, e) =>
            {
                var menu = Storyboard.InstantiateViewController("Menu");
                menu.ModalInPopover = true;
                menu.ModalPresentationStyle = UIModalPresentationStyle.Custom;
                menu.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
                PresentViewControllerAsync(menu, true);
            };
            NavigationItem.LeftBarButtonItem = menuButton;

            Scope.EndScope();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            MessengerInstance.Register<SnapshotsViewModel.NavigateToLoginMessage>(
                this, _ => NavigationController.PushViewController(Storyboard.InstantiateViewController("Login"), true));
        }
    }
}