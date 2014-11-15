using XamarinCommons.Image;
using PCLStorage;
using MonoTouch.UIKit;

namespace Spectator.Ios.Model
{
	public class UIImageDecoder : IImageDecoder
	{
		public object Decode (IFile file)
		{
			return UIImage.FromFile (file.Path);
		}

		public int GetImageSize (ImageWrapper commonImage)
		{
			var image = (UIImage)commonImage?.Image;
			return (int)(image == null ? 0 : image.Size.Width * image.Size.Height * 4);
		}
	}
}