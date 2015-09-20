using UIKit;
using System.Windows.Input;

namespace Spectator.iOS.Common
{
    public class CommandUIActionSheet : UIActionSheet
    {
        public CommandUIActionSheet AddCommand(string title, ICommand command)
        {
            var id = AddButton(title);
            Clicked += (sender, e) =>
            {
                if (e.ButtonIndex == id)
                    command.Execute(null);
            };
            return this;
        }

        public CommandUIActionSheet AddCancelButton(string title)
        {
            CancelButtonIndex = AddButton(title);
            return this;
        }
    }
}