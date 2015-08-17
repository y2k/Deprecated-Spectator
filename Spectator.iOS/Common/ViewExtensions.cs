using System.Windows.Input;
using Foundation;
using UIKit;

namespace Spectator.iOS.Common
{
    static class ViewExtensions
    {
        public static void SetCommand(this UIButton instance, ICommand command)
        {
            var commandButton = instance as CommandButton;
            if (commandButton != null)
                commandButton.Command = command;
            else
                instance.TouchUpInside += (sender, e) => command.Execute(null);
        }

        public static void LoadUrl(this UIWebView instance, string url)
        {
            instance.LoadRequest(new NSUrlRequest(new NSUrl(url ?? "about:blank")));
        }
    }
}