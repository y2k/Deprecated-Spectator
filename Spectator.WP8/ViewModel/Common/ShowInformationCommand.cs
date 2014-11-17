using Microsoft.Phone.Shell;
using System.Windows;

namespace Spectator.WP8.ViewModel.Common
{
    class InformationToast
    {
        string message;

        public InformationToast(string message)
        {
            this.message = message;
        }

        public void Show()
        {
            MessageBox.Show(message);
        }
    }
}