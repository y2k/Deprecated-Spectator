using System;
using UIKit;
using GalaSoft.MvvmLight.Messaging;

namespace Spectator.iOS.Common
{
    public class BaseUIViewController : UIViewController
    {
        protected Scope Scope { get; private set; }

        protected IMessenger MessengerInstance { get { return Messenger.Default; } }

        public BaseUIViewController(IntPtr handle)
            : base(handle)
        {
            Scope = new Scope();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            MessengerInstance.Unregister(this);
        }
    }
}