using System;
using System.Text;
using Spectator.Core.Model.Web;
using Microsoft.Practices.ServiceLocation;

namespace Spectator.Core.Model
{
    public class ImageIdToUrlConverter
    {
        ISpectatorApi api = ServiceLocator.Current.GetInstance<ISpectatorApi>();

        public string Convert(int imageId)
        {
            if (imageId <= 0)
                return null;

            var url = new StringBuilder("" + api.CreateFullUrl("Image/Index/"));
            url.Append(imageId);
            return url.ToString();
        }

        public string ToThumbnailUri(string originalUri, int maxSizePx)
        {
            if (originalUri == null)
                return null;

            var url = new StringBuilder(originalUri);
            url.Append("?width=" + maxSizePx);
            url.Append("&height=" + maxSizePx);
            return url.ToString();
        }

        public string GetThumbnailUrl(int imageId, int maxSizePx)
        {
            if (imageId <= 0)
                return null;

            var url = new StringBuilder("" + api.CreateFullUrl("Image/Thumbnail/"));
            url.Append(imageId);
            url.Append("?width=" + maxSizePx);
            url.Append("&height=" + maxSizePx);
            return url.ToString();
        }

        public Uri CreateThumbnailUrl(Uri url, int px)
        {
            if (px == 0)
                return url;
		
            var s = string.Format("/Cache/Get?maxHeight=500&width={0}&url={1}", px, Uri.EscapeDataString("" + url));
            return api.CreateFullUrl(s);
        }
    }
}