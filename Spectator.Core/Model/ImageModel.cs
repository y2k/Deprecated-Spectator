using System;
using Microsoft.Practices.ServiceLocation;
using XamarinCommons.Image;
using System.Text;

namespace Spectator.Core.Model
{
	public class ImageModel
	{
		PlatformEnvironment platform = ServiceLocator.Current.GetInstance<PlatformEnvironment> ();

		ImageDownloader imageDownloader = new ImageDownloader {
			Decoder = ServiceLocator.Current.GetInstance<IImageDecoder> (),
			DiskCache = new DefaultDiskCache (),
			MemoryCache = new DefaultMemoryCache (),
		};

		public async void Load (object token, Uri originalUri, int maxWidth, Action<ImageWrapper> callback)
		{
			var image = await imageDownloader.LoadAsync (token, CreateThumbnailUrl (originalUri, maxWidth));
			if (image != ImageWrapper.Invalide)
				callback (image);
		}

		public string GetThumbnailUrl (int imageId, int maxWidthPx)
		{
			if (imageId <= 0)
				return null;

			var url = new StringBuilder (Constants.BaseApi + "Image/Thumbnail/");
			url.Append (imageId);
			url.Append ("?width=" + maxWidthPx);
			url.Append ("&height=" + maxWidthPx);
			if (platform.SupportWebp)
				url.Append ("&type=webp");
			return url.ToString ();
		}

		Uri CreateThumbnailUrl (Uri url, int px)
		{
			if (px == 0)
				return url;

			var s = string.Format (
				        "http://remote-cache.api-i-twister.net/Cache/Get?maxHeight=500&width={0}&url={1}", 
				        px, Uri.EscapeDataString ("" + url));
			return new Uri (s);
		}
	}
}