using UIKit;

namespace Spectator.iOS.Common
{
    public class SideMenu
    {
        readonly UIViewController parent;
        readonly UIBarButtonItem menuButton;
        readonly string menuStoryboardId;
        readonly UIView parentView;

        UIButton closeButton;
        UIView menuView;

        public SideMenu(UIViewController parent, string menuStoryboardId)
        {
            this.menuStoryboardId = menuStoryboardId;
            this.parent = parent;
            parentView = parent.NavigationController.View;

            menuButton = new UIBarButtonItem { Image = UIImage.FromBundle("ic_menu_white.png") };
            menuButton.Clicked += (sender, e) => MenuButtonClicked();
        }

        void MenuButtonClicked()
        {
            menuView = parent.Storyboard.InstantiateViewController(menuStoryboardId).View;
            var menuFrame = parentView.Frame;
            menuFrame.Width = 280;
            menuView.Frame = menuFrame;
            parentView.AddSubview(menuView);
            parentView.SendSubviewToBack(menuView);
            menuFrame.X = -280;

            closeButton = new UIButton(parentView.Frame);
            closeButton.TouchUpInside += (sender, e) => CloseButtonClicked();
            parentView.AddSubview(closeButton);

            UIView.Animate(0.3,
                () =>
                {
                    menuView.Frame = menuFrame;
                    foreach (var s in parentView.Subviews)
                    {
                        var f = s.Frame;
                        f.Offset(280, 0);
                        s.Frame = f;
                    }
                });
        }

        async void CloseButtonClicked()
        {
            await UIView.AnimateAsync(0.3,
                () =>
                {
                    foreach (var s in parentView.Subviews)
                    {
                        if (s == menuView)
                            continue;
                        var f = s.Frame;
                        f.Offset(-280, 0);
                        s.Frame = f;
                    }
                });

            closeButton.RemoveFromSuperview();
            menuView.RemoveFromSuperview();

            closeButton = null;
            menuView = null;
        }

        public void Attach()
        {
            parent.NavigationItem.LeftBarButtonItem = menuButton;
        }
    }
}