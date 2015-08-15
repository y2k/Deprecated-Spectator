using UIKit;

namespace Spectator.iOS.Common
{
    public class SideMenu
    {
        readonly UIViewController parent;
        readonly UIBarButtonItem menuButton;

        public SideMenu(UIViewController parent, string menuStoryboardId)
        {
            this.parent = parent;
            menuButton = new UIBarButtonItem { Image = UIImage.FromBundle("ic_menu_white.png") };
            menuButton.Clicked += (sender, e) =>
            {
                var menu = parent.Storyboard.InstantiateViewController(menuStoryboardId);
                menu.ModalInPopover = true;
                menu.ModalPresentationStyle = UIModalPresentationStyle.Custom;
                menu.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
                parent.PresentViewControllerAsync(menu, true);
            };
        }

        public void Attach()
        {
            parent.NavigationItem.LeftBarButtonItem = menuButton;
        }
    }
}