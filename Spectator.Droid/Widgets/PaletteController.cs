using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Graphics;
using Color = global::Android.Graphics.Color;
using Android.Support.V7.Graphics;

namespace Spectator.Droid.Widgets
{
	public class PaletteController
	{
		List<Item> items = new List<Item> ();
		IDictionary<string, Palette> cache;
		WeakReference<WebImageView> image;

		PaletteController (WebImageView image, IDictionary<string, Palette> cache)
		{
			image.ImageChanged += HandleImageChanged;
			image.ImageSourceChanged += HandleImageSourceChanged;
			this.cache = cache;
			this.image = new WeakReference<WebImageView> (image);
		}

		void HandleImageSourceChanged (object sender, string imageSource)
		{
			Palette p;
			if (imageSource != null) {
				cache.TryGetValue (imageSource, out p);
				UpdatePalette (p);
			}
		}

		async void HandleImageChanged (object sender, Bitmap image)
		{
			var p = await Task.Run (() => image == null ? null : Palette.Generate (image));

			WebImageView iv;
			this.image.TryGetTarget (out iv);
			string source = iv == null ? null : iv.ImageSource;
			if (p != null && source != null)
				cache [source] = p;

			UpdatePalette (p);
		}

		void UpdatePalette (Palette p)
		{
			if (p != null) {
				foreach (var s in items) {
					var i = s.selector (p);
					if (i != null)
						s.callback (s.view, i);
				}
			}
		}

		public static Color InvertColor (Color color)
		{
			var hsv = new float[3];
			Color.ColorToHSV (color, hsv);
			hsv [0] = (180 + hsv [0]) % 360;
			hsv [1] = 1 - hsv [1];
			hsv [2] = 1 - hsv [2];
			return Color.HSVToColor (hsv);
		}

		public void AddView<T> (T view, Func<Palette, Palette.Swatch> selector, Action<T, Palette.Swatch> callback)
		{
			items.Add (new Item { view = view, selector = selector, callback = (o, it) => callback ((T)o, it) });
		}

		class Item
		{
			internal object view;
			internal Func<Palette, Palette.Swatch> selector;
			internal Action<object, Palette.Swatch> callback;
		}

		public class Fabric
		{
			IDictionary<string, Palette> cache = new Dictionary<string, Palette> ();

			public PaletteController NewInstance (WebImageView imageView)
			{
				return new PaletteController (imageView, cache);
			}
		}
	}
}