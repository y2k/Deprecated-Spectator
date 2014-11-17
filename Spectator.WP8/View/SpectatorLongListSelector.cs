using Microsoft.Phone.Controls;
using System.Windows.Controls;

namespace Spectator.WP8.View
{
    public class SpectatorLongListSelector : LongListSelector
    {
        public SpectatorLongListSelector()
        {
            SelectionChanged += SpectatorLongListSelector_SelectionChanged;
        }

        private void SpectatorLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] != null)
                SelectedItem = null;
        }
    }
}