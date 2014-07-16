using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Graphics;
using Android.Graphics;
using System.Threading.Tasks;
using Color = global::Android.Graphics.Color;

namespace Spectator.Android.Application.Widget
{
	public class PaletteController
	{
		private List<Item> items = new List<Item> ();
		private IDictionary<string, Palette> cache;
		private WeakReference<WebImageView> image;

		private PaletteController (WebImageView image, IDictionary<string, Palette> cache)
		{
//			image.ImageChanged += HandleImageChanged;
//			image.ImageSourceChanged += HandleImageSourceChanged;
//			this.cache = cache;
//			this.image = new WeakReference<WebImageView> (image);
		}

		private void HandleImageSourceChanged (object sender, string imageSource)
		{
//			Palette p;
//			cache.TryGetValue(imageSource, out p);
//			UpdatePalette (p);
		}

		private async void HandleImageChanged (object sender, Bitmap image)
		{
//			var p = await Task.Run (() => image == null ? null : Palette.Generate (image));
//
//			WebImageView iv;
//			this.image.TryGetTarget (out iv);
//			string source = iv == null ? null : iv.ImageSource;
//			if (p != null && source != null) cache [source] = p;
//
//			UpdatePalette (p);
		}

		private void UpdatePalette (Palette p)
		{
//			if (p != null) {
//				foreach (var s in items) {
//					var i = s.selector (p);
//					if (i != null)
//						s.callback (s.view, i);
//				}
//			}
		}

		public void AddView<T> (T view, Func<Palette, PaletteItem> selector, Action<T, PaletteItem> callback)
		{
//			items.Add (new Item { view = view, selector = selector, callback = (o, it) => callback ((T)o, it) });
		}

		private class Item
		{
			internal object view;
			internal Func<Palette, PaletteItem> selector;
			internal Action<object, PaletteItem> callback;
		}

		public class Fabric
		{
			private IDictionary<string, Palette> cache = new Dictionary<string, Palette> ();

			public PaletteController NewInstance (WebImageView imageView)
			{
				return new PaletteController (imageView, cache);
			}
		}
	}
}