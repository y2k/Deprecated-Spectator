using XamarinCommons.Image;
using PCLStorage;
using MonoTouch.UIKit;

namespace Spectator.Ios.Model
{
	public class UIImageDecoder : ImageDecoder
	{
		public override int GetImageSize (object commonImage)
		{
			var image = commonImage as UIImage;
			return (int)(image == null ? 0 : image.Size.Width * image.Size.Height * 4);
		}

		public override object Decode (IFile file)
		{
			return UIImage.FromFile (file.Path);
		}
	}
}