﻿using UIKit;

namespace Spectator.iOS.Common
{
    public class SideMenu
    {
        const float PanelWidth = 280;

        readonly UIViewController parent;
        readonly UIView parentView;

        readonly UIButton closeButton;
        readonly UIView menuView;

        public SideMenu(UIViewController parent, string menuStoryboardId)
        {
            this.parent = parent;
            parentView = parent.NavigationController.View;
            menuView = parent.Storyboard.InstantiateViewController(menuStoryboardId).View;

            closeButton = new UIButton(parentView.Frame);
            closeButton.TouchUpInside += (sender, e) => CloseButtonClicked();
        }

        async void CloseButtonClicked()
        {
            await UIView.AnimateAsync(0.3, RestoveViewPosition);
            RemoveMenuViews();
        }

        public void Attach()
        {
            var menuButton = new UIBarButtonItem { Image = UIImage.FromBundle("ic_menu_white.png") };
            menuButton.Clicked += (sender, e) => MenuButtonClicked();
            parent.NavigationItem.LeftBarButtonItem = menuButton;

            var edgeGesture = new UIScreenEdgePanGestureRecognizer(MenuButtonClicked);
            edgeGesture.Edges = UIRectEdge.Left;
            parent.View.AddGestureRecognizer(edgeGesture);
        }

        void MenuButtonClicked()
        {
            if (menuView.Superview != null)
                return;

            var menuFrame = parentView.Frame;
            menuFrame.Width = PanelWidth;
            menuView.Frame = menuFrame;
            parentView.AddSubview(menuView);
            parentView.SendSubviewToBack(menuView);
            menuFrame.X = -PanelWidth;

            parentView.AddSubview(closeButton);

            UIView.Animate(0.3,
                () =>
                {
                    menuView.Frame = menuFrame;
                    foreach (var s in parentView.Subviews)
                    {
                        var f = s.Frame;
                        f.Offset(PanelWidth, 0);
                        s.Frame = f;
                    }
                });
        }

        public void Activate()
        {
            // TODO:
        }

        public void Deactive()
        {
            if (closeButton.Superview == null)
                return;
            RestoveViewPosition();
            RemoveMenuViews();
        }

        void RestoveViewPosition()
        {
            foreach (var s in parentView.Subviews)
            {
                if (s == menuView)
                    continue;
                var f = s.Frame;
                f.Offset(-PanelWidth, 0);
                s.Frame = f;
            }
        }

        void RemoveMenuViews()
        {
            closeButton.RemoveFromSuperview();
            menuView.RemoveFromSuperview();
        }
    }
}