using System;
using GalaSoft.MvvmLight.Messaging;
using Spectator.Core.ViewModels.Common;
using UIKit;

namespace Spectator.iOS.Common
{
    public class BaseUIViewController : UIViewController
    {
        protected Scope Scope { get; private set; }

        protected IMessenger MessengerInstance { get { return Messenger.Default; } }

        public NavigationMessage Argument { get; set; }

        public BaseUIViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Scope = new Scope(Argument);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            MessengerInstance.Unregister(this);
        }
    }
}