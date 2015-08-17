using UIKit;
using Spectator.Core.ViewModels.Common;

namespace Spectator.iOS.Common
{
    public static class UIControllerExtension
    {
        public static void PushViewController(this UIViewController instance, string storyboardId)
        {
            var vc = instance.Storyboard.InstantiateViewController(storyboardId);
            instance.NavigationController.PushViewController(vc, true);
        }

        public static void PushViewController(this UIViewController instance, string storyboardId, NavigationMessage argument)
        {
            var vc = (BaseUIViewController)instance.Storyboard.InstantiateViewController(storyboardId);
            vc.Argument = argument;
            instance.NavigationController.PushViewController(vc, true);
        }

        public static void ReplaceViewController(this UIViewController instance, string storyboardId)
        {
            var vc = instance.Storyboard.InstantiateViewController(storyboardId);
            instance.NavigationController.SetViewControllers(new [] { vc }, true);
        }
    }
}