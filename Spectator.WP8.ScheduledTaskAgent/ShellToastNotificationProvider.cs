using Microsoft.Phone.Shell;
using Spectator.Core.Model.Push;

namespace Spectator.WP8.ScheduledTaskAgent
{
    class ShellToastNotificationService : INotificationService
    {
        public void ShowHasNewMessage(int updatedSubscriptions, int totalNewShapshots)
        {
            new ShellToast()
            {
                Title = "Spectator has new snapshots",
                Content = "Updated subscriptions " + updatedSubscriptions + "\r\nTotal new snapshots " + totalNewShapshots,
            }.Show();
        }
    }
}
