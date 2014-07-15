
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

namespace Spectator.Android.Application.Widget
{
	public class PaletteController
	{
		private List<Item> items = new List<Item> ();

		public PaletteController (WebImageView image)
		{
			image.ImageChanged += HandleImageChanged;
		}

		private async void HandleImageChanged (object sender, Bitmap image)
		{
			var p = await Task.Run (() => image == null ? null : Palette.Generate (image));
			if (p != null) {
				foreach (var s in items) {
					var i = s.selector (p);
					if (i != null)
						s.callback (s.view, i);
				}
			}
		}

		public void AddView<T> (T view, Func<Palette, PaletteItem> selector, Action<T, PaletteItem> callback)
		{
			items.Add (new Item { view = view, selector = selector, callback = (o, it) => callback ((T)o, it) });
		}

		private class Item
		{

			internal object view;
			internal Func<Palette, PaletteItem> selector;
			internal Action<object, PaletteItem> callback;
		}
	}
}