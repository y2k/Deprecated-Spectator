using System;
using Spectator.Core.ViewModels;
using Spectator.iOS.Common;

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