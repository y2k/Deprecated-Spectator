using System.IO;
using Android.Graphics;
using XamarinCommons.Image;

namespace Spectator.Android.Application.Model
{
	public class BitmapImageDecoder : ImageDecoder
	{
		public override object DecoderStream (Stream stream)
		{
			return BitmapFactory.DecodeStream (stream);
		}

		public override int GetImageSize (object commonImage)
		{
			return ((Bitmap)commonImage).ByteCount;
		}
	}
}