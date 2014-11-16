using System;
using XamarinCommons.Image;

namespace Spectator.WP8.Model
{
    public class StubImageDecoder : ImageDecoder
    {
        public override int GetImageSize(object commonImage)
        {
            throw new NotImplementedException();
        }
    }
}