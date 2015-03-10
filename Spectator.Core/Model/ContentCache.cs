using PCLStorage;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Spectator.Core.Model
{
    class ContentCache
	{
		public async Task SaveToCache (Uri dataUrl)
		{
			var name = CreateLocalPathForUri (dataUrl);
			if (await FileSystem.Current.LocalStorage.CheckExistsAsync (name) == ExistenceCheckResult.FileExists)
				return;
			var path = await FileSystem.Current.LocalStorage.CreateFileAsync (name, CreationCollisionOption.FailIfExists);

			using (var dataStream = await new HttpClient ().GetStreamAsync (dataUrl)) {
				using (var destFile = await path.OpenAsync (FileAccess.ReadAndWrite))
					destFile.WriteAllStream (dataStream);
			}
		}

		public async Task<string> LoadFromCache (Uri dataUrl)
		{
			var name = CreateLocalPathForUri (dataUrl);
			var file = await FileSystem.Current.LocalStorage.GetFileAsync (name);
			return await file.ReadAllTextAsync ();
		}

		string CreateLocalPathForUri (Uri dataUrl)
		{
			return "content_" + dataUrl.GetHashCode () + ".html";
		}
	}
}