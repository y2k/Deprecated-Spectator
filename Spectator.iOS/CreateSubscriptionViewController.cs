using System;
using Spectator.iOS.Common;
using Spectator.Core.ViewModels;

namespace Spectator.iOS
{
    public partial class CreateSubscriptionViewController : BaseUIViewController
    {
        public CreateSubscriptionViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CancelButton.Clicked += (sender, e) => DismissViewController(true, null);

            var viewmodel = Scope.New<CreateSubscriptionViewModel>();

            DoneButton.SetCommand(viewmodel.CreateCommand);

            Title.SetBinding((s, v) => s.Text = v, () => viewmodel.Title).SetTwoWay();
            Url.SetBinding((s, v) => s.Text = v, () => viewmodel.Link).SetTwoWay();

            Scope.EndScope();
        }
    }
}