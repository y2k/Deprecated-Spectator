using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Image;

namespace Spectator.Android.Application.Widget
{
	public class WebImageView : ImageView
	{
		ImageModel iModel = ServiceLocator.Current.GetInstance<ImageModel> ();

		string imageSource;

		public event EventHandler<Bitmap> ImageChanged;
		public event EventHandler<string> ImageSourceChanged;

		public string ImageSource {
			get { return imageSource; }
			set { UpdateImageSource (value); }
		}

		public WebImageView (Context context, global::Android.Util.IAttributeSet attrs) : base (context, attrs)
		{
		}

		void UpdateImageSource (string imageSource)
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
						SetImageBitmap ((Bitmap)s.Image);
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
			if (old != null)
				old.Dispose ();

			base.SetImageDrawable (drawable);
		}
	}
}