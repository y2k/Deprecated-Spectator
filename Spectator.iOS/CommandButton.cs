using System;
using UIKit;
using System.Windows.Input;

namespace Spectator.iOS
{
    public partial class CommandButton : UIButton
    {
        public ICommand Command { get; set; }

        public CommandButton(IntPtr handle)
            : base(handle)
        {
            TouchUpInside += (sender, e) => Command.Execute(null);
        }
    }
}