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
        static readonly UriNormalizer Normalizer = new UriNormalizer();
        static readonly DiskCache DiskCache = new DiskCache();
        static readonly MemoryCache MemoryCache = new MemoryCache();

        string url;
        int size;

        public BaseImageRequest SetUri(string url)
        {
            this.url = url;
            return this;
        }

        public BaseImageRequest SetImageSize(int size)
        {
            this.size = size;
            return this;
        }

        public void To(object target)
        {
            new Downloader { parent = this, uri = Normalizer.Normalize(url, size) }.To(target);
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
                    if (IsInvalideState())
                        return;
                    if (cachedBytes == null)
                    {
                        byte[] data;
                        try
                        {
                            data = await Execute(uri);
                        }
                        catch (Exception e)
                        {
                            #if DEBUG
                            throw e;
                            #else
                            Xamarin.Insights.Report(e, "Url", "" + uri);
                            return;
                            #endif
                        }

                        if (IsInvalideState())
                            return;
                        await DiskCache.PutAsync(uri, data);
                        if (IsInvalideState())
                            return;
                        var image = await parent.DecodeImageAsync(data);
                        if (IsInvalideState())
                            return;
                        MemoryCache.Put(uri, image);
                        parent.SetToTarget(target, image);
                    }
                    else
                    {
                        var image = await parent.DecodeImageAsync(cachedBytes);
                        if (IsInvalideState())
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

            bool IsInvalideState()
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

        class UriNormalizer
        {
            ImageIdToUrlConverter converter = new ImageIdToUrlConverter();

            internal Uri Normalize(string url, int size)
            {
                return new Uri(converter.GetThumbnailUrl(int.Parse(url), size));
            }
        }
    }
}