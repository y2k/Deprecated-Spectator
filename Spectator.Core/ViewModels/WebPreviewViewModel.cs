using System;

namespace Spectator.Core.ViewModels
{
    public class WebPreviewViewModel : ViewModel
    {
        public string Url { get; set; }

        public Mode CurrentMode { get; set; }

        public WebPreviewViewModel()
        {
        }

        public enum Mode
        {
            WebPreview,
            Difference
        }
    }
}