using System;
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
            var imageView = ((UIImageView)target);
            imageView.Image = (UIImage)image;

            imageView.Alpha = 0;
            UIView.Animate(0.3, () => imageView.Alpha = 1);
        }

        public BaseImageRequest SetImageSize(nfloat width, nfloat height)
        {
            return base.SetImageSize((float)width, (float)height);
        }

        public BaseImageRequest SetImageSize(nfloat size)
        {
            return SetImageSize(size, size);
        }
    }
}