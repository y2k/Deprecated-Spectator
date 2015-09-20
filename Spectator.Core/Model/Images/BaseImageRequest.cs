using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Spectator.Core.Model.Images
{
    public abstract class BaseImageRequest
    {
        protected abstract Task<object> DecodeImageAsync(byte[] data);

        protected abstract void SetToTarget(object target, object image);

        static readonly OperationTransaction Transaction = new OperationTransaction();
        static readonly DiskCache DiskCache = new DiskCache();
        static readonly MemoryCache MemoryCache = new MemoryCache();

        readonly UriBuilder uriBuilder = new UriBuilder();

        public BaseImageRequest SetUri(string url)
        {
            uriBuilder.Url = url;
            return this;
        }

        public BaseImageRequest SetImageSize(float width, float height)
        {
            uriBuilder.Width = (int)width;
            uriBuilder.Hieight = (int)height;
            return this;
        }

        public void To(object target)
        {
            new Downloader { parent = this, uri = uriBuilder.Build() }.To(target);
        }

        class Downloader
        {
            static readonly HttpClient Client = new HttpClient();

            internal Uri uri;
            internal BaseImageRequest parent;

            public async void To(object target)
            {
                {
                    var image = MemoryCache.Get(uri);
                    if (image != null)
                    {
                        parent.SetToTarget(target, image);
                        return;
                    }
                }

                BaseImageRequest.Transaction.Begin(target, parent);
                parent.SetToTarget(target, null);
                try
                {
                    var cachedBytes = await DiskCache.GetAsync(uri);
                    if (IsInvalidState())
                        return;
                    if (cachedBytes == null)
                    {
                        byte[] data = null;
                        for (int n = 0; n < 5; n++)
                        {
                            try
                            {
                                data = await Execute(uri);
                                if (IsInvalidState())
                                    return;
                                break;
                            }
                            catch
                            {
                                await Task.Delay(500 << n);
                            }
                        }
                        if (data == null)
                            return;

                        if (IsInvalidState())
                            return;
                        await DiskCache.PutAsync(uri, data);
                        if (IsInvalidState())
                            return;
                        var image = await parent.DecodeImageAsync(data);
                        if (IsInvalidState())
                            return;
                        MemoryCache.Put(uri, image);
                        parent.SetToTarget(target, image);
                    }
                    else
                    {
                        var image = await parent.DecodeImageAsync(cachedBytes);
                        if (IsInvalidState())
                            return;
                        MemoryCache.Put(uri, image);
                        parent.SetToTarget(target, image);
                    }
                }
                finally
                {
                    BaseImageRequest.Transaction.End(target, parent);
                }
            }

            Task<byte[]> Execute(Uri uri)
            {
                return Client.GetByteArrayAsync(uri);
            }

            bool IsInvalidState()
            {
                return !BaseImageRequest.Transaction.IsValid(parent);
            }
        }

        class OperationTransaction
        {
            readonly Dictionary<object, BaseImageRequest> LockedTargets = new Dictionary<object, BaseImageRequest>();

            internal void Begin(object target, BaseImageRequest requste)
            {
                LockedTargets[target] = requste;
            }

            internal void End(object target, BaseImageRequest requste)
            {
                if (IsValid(requste))
                    LockedTargets.Remove(target);
            }

            internal bool IsValid(BaseImageRequest requste)
            {
                return LockedTargets.ContainsValue(requste);
            }
        }

        class UriBuilder
        {
            const string ThumbnailTemplate = "http://spectator-api.cloudapp.net/api/images/{0}?bgColor=ffffff&width={1}&height={2}";

            public string Url { get; set; }

            public int Width { get; set; }

            public int Hieight { get; set; }

            public Uri Build()
            {
                return new Uri(string.Format(ThumbnailTemplate, Url, Width, Hieight));
            }
        }
    }
}