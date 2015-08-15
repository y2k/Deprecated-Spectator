using System.Threading.Tasks;
using Foundation;
using Spectator.Core.Model.Images;
using UIKit;

namespace Spectator.iOS.Platform
{
    public class ImageRequest : BaseImageRequest
    {
        protected override Task<object> DecodeImageAsync(byte[] data)
        {
            return Task.Run<object>(() => new UIImage(NSData.FromArray(data)));
        }

        protected override void SetToTarget(object target, object image)
        {
            ((UIImageView)target).Image = (UIImage)image;
        }
    }
}