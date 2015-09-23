using System;
using System.IO;
using System.Threading.Tasks;
using Nito.AsyncEx;
using PCLStorage;

namespace Spectator.Core.Model.Images
{
    public class DiskCache
    {
        readonly ImageFolder imageFolder = new ImageFolder();
        readonly AsyncReaderWriterLock accessLock = new AsyncReaderWriterLock();

        public async Task<byte[]> GetAsync(Uri uri)
        {
            using (await accessLock.ReaderLockAsync())
            {
                var folder = await imageFolder.GetAsync();
                if (await folder.CheckExistsAsync("" + uri.GetHashCode()) != ExistenceCheckResult.FileExists)
                    return null;

                var file = await folder.GetFileAsync("" + uri.GetHashCode());
                var result = new MemoryStream();
                using (var stream = await file.OpenAsync(FileAccess.ReadAndWrite))
                    await stream.CopyToAsync(result);
                return result.ToArray();
            }
        }

        public async Task PutAsync(Uri uri, byte[] data)
        {
            using (await accessLock.WriterLockAsync())
            {
                var folder = await imageFolder.GetAsync();
                var file = await folder.CreateFileAsync("" + uri.GetHashCode(), CreationCollisionOption.ReplaceExisting);
                using (var stream = await file.OpenAsync(FileAccess.ReadAndWrite))
                    await stream.WriteAsync(data, 0, data.Length);
            }
        }

        Task<IFolder> GetFolder()
        {
            return imageFolder.GetAsync();
        }

        class ImageFolder
        {
            IFolder folder;

            public async Task<IFolder> GetAsync()
            {
                if (folder == null)
                    folder = await CreateFolder();
                return folder;
            }

            static Task<IFolder> CreateFolder()
            {
                return FileSystem.Current.LocalStorage.CreateFolderAsync(
                    "images", CreationCollisionOption.OpenIfExists);
            }
        }
    }
}