﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using XamarinCommons.Image;

namespace Spectator.Core.Model.Image
{
	public class ImageModel
	{
//		const int MaxAttempts = 5;
//		const int BaseAttemptDelay = 500;
//
//		IMemoryCache memoryCache = ServiceLocator.Current.GetInstance<IMemoryCache> ();
//		IDiskCache diskCachge = ServiceLocator.Current.GetInstance<IDiskCache> ();
//		HttpClient webClient = new HttpClient ();
//
//		Dictionary<object, Uri> lockedImages = new Dictionary<object, Uri> ();
//
//		#region IImageModel implementation
//
//		public async void Load (object token, Uri originalUri, int maxWidth, Action<object> originalImageCallback)
//		{
//			if (originalUri == null) {
//				originalImageCallback (null);
//				lockedImages.Remove (token);
//				return;
//			}
//
//			lockedImages [token] = originalUri;
//			Action<object> imageCallback = image => {
//				if (lockedImages.Any (s => s.Key == token && s.Value == originalUri)) {
//					originalImageCallback (image);
//					lockedImages.Remove (token);
//				}
//			};
//
//			var uri = CreateThumbnailUrl (originalUri, maxWidth);
//
//			// Поиск картинки в кэше памяти
//			var mi = memoryCache.Get (uri);
//			if (mi != null) {
//				imageCallback (mi);
//				return;
//			}
//
//			#if ACCESS_TO_DISK_IN_MAIN
//			// Поиск картинки в кэше на диске
//			if (Math.Abs(1) == 0) { // FIXME
//				// Запрос к диску в главном потоке 
//				var i = diskCachge.Get (uri);
//				if (i != null) {
//					memoryCache.Put (uri, i);
//					imageCallback(i);
//					return;
//				}
//			} else {
//			#endif
//			// Запрос к диску в фоновом потоке
//			var i = await Task.Run<object> (() => diskCachge.Get (uri));
//			if (i != null) {
//				memoryCache.Put (uri, i);
//				imageCallback (i);
//				return;
//			}
//			#if ACCESS_TO_DISK_IN_MAIN
//			}
//			#endif
//
//			// Загрузка картинки с вэба
//			await Task.Run (
//				async () => {
//
//					for (int t = 0; t < MaxAttempts; t++) {
//						try {
//							using (var ins = await webClient.GetStreamAsync (uri)) {
//								diskCachge.Put (uri, ins);
//								mi = diskCachge.Get (uri);
//								if (mi != null)
//									memoryCache.Put (uri, mi);
//							}
//							return;
//						} catch (HttpRequestException) {
//							new ManualResetEvent (false).WaitOne (BaseAttemptDelay << t);
//						}
//					}
//				
//				});
//
//			imageCallback (mi);
//		}
//
//		#endregion

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