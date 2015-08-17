using Spectator.Core.Model;

namespace Spectator.Core.ViewModels
{
    public class WebPreviewViewModel : ViewModel
    {
        public string Url { get { return Get<string>(); } set { Set(value); } }

        public Mode CurrentMode { get { return Get<Mode>(); } set { Set(value); } }

        public async void Initialize(SnapshotsViewModel.NavigateToWebPreview argument)
        {
            CurrentMode = Mode.WebPreview;

            var service = new SnapshotService(argument.SnashotId);
            await service.SyncWithWeb();
            Url = await service.GetContent();
        }

        public enum Mode
        {
            WebPreview,
            Difference
        }
    }
}