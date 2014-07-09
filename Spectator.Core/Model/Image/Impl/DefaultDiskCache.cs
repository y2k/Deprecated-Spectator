using System;
using PCLStorage;
using System.IO;
using System.Threading;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Image;

namespace Spectator.Core.Model.Image.Impl
{
	public class DefaultDiskCache : IDiskCache
	{
		private IImageDecoder decoder = ServiceLocator.Current.GetInstance<IImageDecoder> ();
		private IFolder root;

		public DefaultDiskCache ()
		{
			root = FileSystem.Current.LocalStorage;
		}

		#region IDiskCache implementation

		public ImageWrapper Get (Uri uri)
		{
			var name = ConvertUriToFilename (uri);
			if (root.CheckExistsAsync (name).Result == ExistenceCheckResult.NotFound) return null;
			var f = root.GetFileAsync (name).Result;

			object image = null;
			using (var stream = f.OpenAsync (FileAccess.Read).Result) {
				image = decoder.Decode (stream);
			}

			if (image == null) {
				f.DeleteAsync ().Wait ();
				return null;
			}
			return new ImageWrapper { Image = image };
		}

		public void Put (Uri uri, Stream image)
		{
			var file = ConvertUriToFilename (uri);
			if (root.CheckExistsAsync (file).Result == ExistenceCheckResult.FileExists)
				return;

			var tn = Guid.NewGuid () + ".tmp";
			var tmp = root.CreateFileAsync (tn, CreationCollisionOption.ReplaceExisting).Result;
			using (var outs = tmp.OpenAsync (FileAccess.ReadAndWrite).Result) {
				var buf = new byte[4 * 1024];
				int count;
				while ((count = image.Read(buf, 0, buf.Length)) > 0) {
					outs.Write(buf, 0, count);
				}
			}

			try {
				tmp.RenameAsync (file).Wait();
			} catch {
				tmp.DeleteAsync ().Wait ();
			}
		}

		#endregion

		private string ConvertUriToFilename(Uri uri) 
		{
			return uri.GetHashCode () + ".bin";
		}
	}
}