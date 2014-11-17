using System.Windows;

namespace Spectator.WP8.ViewModel.Common
{
    class ErrorToast
    {
        private string message;

        public ErrorToast(string message)
        {
            this.message = message;
        }

        public void Show()
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK);
        }
    }
}