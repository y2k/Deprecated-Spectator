using System;
using System.IO;
using Android.Graphics;
using Spectator.Core.Model.Image;

namespace Spectator.Android.Application.Model
{
	public class BitmapImageDecoder : IImageDecoder
	{
		#region ImageDecoder implementation

		public object Decode (Stream stream)
		{
			return BitmapFactory.DecodeStream (stream);
		}

		public int GetImageSize (object commonImage)
		{
			return ((Bitmap)commonImage).ByteCount;
		}

		#endregion
	}
}