using System;
using Spectator.iOS.Common;
using Spectator.Core.ViewModels;
using Foundation;

namespace Spectator.iOS
{
    public partial class LoginViewController : BaseUIViewController
    {
        public LoginViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var viewmodel = Scope.New<LoginViewModel>();

            WebView.SetBinding(
                (s, v) => s.LoadRequest(new NSUrlRequest(new NSUrl(v))),
                () => viewmodel.BrowserUrl);

            WebView.LoadFinished += (sender, e) => 
                viewmodel.BrowserTitle = WebView.EvaluateJavascript("document.title");

            Scope.EndScope();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            MessengerInstance.Register<LoginViewModel.NavigateToHomeMessage>(
                this, _ => NavigationController.SetViewControllers(
                    new [] { Storyboard.InstantiateViewController("Main") }, true));
        }
    }
}