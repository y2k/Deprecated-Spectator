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
using Android.Graphics;
using Spectator.Core.Model;
using Microsoft.Practices.ServiceLocation;
using Android.Graphics.Drawables;

namespace Spectator.Android.Application.Widget
{
	public class WebImageView : ImageView
	{
		private IImageModel iModel = ServiceLocator.Current.GetInstance<IImageModel> ();

		private string imageSource;

		public event EventHandler<Bitmap> ImageChanged;
		public event EventHandler<string> ImageSourceChanged;

		public string ImageSource {
			get { return imageSource; }
			set { UpdateImageSource (value); }
		}

		public WebImageView (Context context, global::Android.Util.IAttributeSet attrs) : base (context, attrs)
		{
		}

		private void UpdateImageSource (string imageSource)
		{
			if (this.imageSource != imageSource) {
				this.imageSource = imageSource;

				if (ImageSourceChanged != null)
					ImageSourceChanged (this, imageSource);

				SetImageDrawable (null);

				var u = imageSource == null ? null : new Uri (imageSource); // u == null отменяет закачки
				iModel.Load (this, u, 0, s => {
					if (s == null)
						SetImageDrawable (null);
					else
						SetImageBitmap((Bitmap)s);
				}); 
			}
		}

		public override void SetImageDrawable (Drawable drawable)
		{
			if (Drawable != drawable && ImageChanged != null) {
				ImageChanged (this, drawable is BitmapDrawable ? ((BitmapDrawable)drawable).Bitmap : null);
			}

			// Устранение утечек памяти из-за связки MonoGC-AndroidGC
			var old = Drawable as BitmapDrawable;
			if (old != null) {
				old.Dispose ();
			}

			base.SetImageDrawable (drawable);
		}
	}
}