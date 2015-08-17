using Spectator.Core.Model;
using Spectator.Core.ViewModels.Common;

namespace Spectator.Core.ViewModels
{
    public class WebPreviewViewModel : ViewModel
    {
        public string Url { get; set; }

        public Mode CurrentMode { get; set; }

        public async void Initialize(SnapshotsViewModel.NavigateToWebPreview argument)
        {
            CurrentMode = Mode.WebPreview;

            var service = new SnapshotService(argument.SnashotId);
            Url = await service.GetContent();
        }

        public enum Mode
        {
            WebPreview,
            Difference
        }
    }
}