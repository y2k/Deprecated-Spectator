using System.Windows.Input;
using UIKit;

namespace Spectator.iOS.Common
{
    static class ViewExtensions
    {
        public static void SetCommand(this UIButton instance, ICommand command)
        {
            instance.TouchUpInside += (sender, e) => command.Execute(null);
        }
    }
}