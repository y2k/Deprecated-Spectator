using System;
using System.IO;

namespace Spectator.Core.Model.Image
{
	public interface IImageDecoder
	{
		object Decode(Stream stream);

		int GetImageSize(object commonImage);
	}
}