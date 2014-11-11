using System.IO;
using Android.Graphics;
using XamarinCommons.Image;
using PCLStorage;

namespace Spectator.Android.Application.Model
{
	public class BitmapImageDecoder : IImageDecoder
	{
		public object Decode (IFile file)
		{
			using (var stream = file.OpenAsync (PCLStorage.FileAccess.Read).Result) {
				return BitmapFactory.DecodeStream (stream);
			}
		}

		public int GetImageSize (ImageWrapper commonImage)
		{
			return ((Bitmap)commonImage.Image).ByteCount;
		}
	}
}