using System;
using UIKit;

namespace Spectator.iOS
{
    public partial class CreateSubscriptionViewController : UIViewController
    {
        public CreateSubscriptionViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CancelButton.Clicked += (sender, e) => DismissViewController(true, null);
        }
    }
}