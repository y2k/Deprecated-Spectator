using System;
using XamarinCommons.Image;

namespace Spectator.WP8.ScheduledTaskAgent
{
    public class StubImageDecoder : ImageDecoder
    {
        public override int GetImageSize(object commonImage)
        {
            throw new NotImplementedException();
        }
    }
}