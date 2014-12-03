using System;
using Microsoft.Practices.ServiceLocation;
using XamarinCommons.Image;

namespace Spectator.Core.Model
{
	public class ImageModel
	{
		ImageDownloader imageDownloader = new ImageDownloader {
			Decoder = ServiceLocator.Current.GetInstance<ImageDecoder> (),
			DiskCache = new DefaultDiskCache (),
			MemoryCache = new DefaultMemoryCache (),
		};

		ImageModel ()
		{
		}

		public async void Load (object token, Uri originalUri, int maxWidth, Action<object> callback)
		{
			var image = await imageDownloader.LoadAsync (token, CreateThumbnailUrl (originalUri, maxWidth));
			if (image != ImageDownloader.InvalideImage)
				callback (image);
		}

		Uri CreateThumbnailUrl (Uri originalUri, int maxWidth)
		{
			return new ImageIdToUrlConverter ().CreateThumbnailUrl (originalUri, maxWidth);
		}

		static Lazy<ImageModel> instance = new Lazy<ImageModel> (() => new ImageModel ());

		public static ImageModel Instance {
			get { return instance.Value; }
		}
	}
}