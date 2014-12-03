using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Widget;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model;
using Android.Runtime;

namespace Spectator.Android.Application.Widget
{
	public class WebImageView : ImageView
	{
		ImageModel iModel = ServiceLocator.Current.GetInstance<ImageModel> ();

		public event EventHandler<Bitmap> ImageChanged;
		public event EventHandler<string> ImageSourceChanged;

		string imageSource;
		int maxImageSize;

		public string ImageSource {
			get { return imageSource; }
			set { UpdateImageSource (value); }
		}

		public WebImageView (IntPtr a, JniHandleOwnership b) : base (a, b)
		{
		}

		public WebImageView (Context context) : base (context)
		{
		}

		public WebImageView (Context context, IAttributeSet attrs) : base (context, attrs)
		{
			for (int i = 0; i < attrs.AttributeCount; i++) {
				if ("max_image_size" == attrs.GetAttributeName (i))
					maxImageSize = attrs.GetAttributeIntValue (i, 0);
			}
		}

		void UpdateImageSource (string originalImageSource)
		{
			var imageSource = NormalizeUri (originalImageSource);

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
						SetImageBitmap ((Bitmap)s);
				}); 
			}
		}

		string NormalizeUri (string imageSource)
		{
			if (maxImageSize > 0)
				imageSource = new ImageIdToUrlConverter ().ToThumbnailUri (imageSource, maxImageSize);
			return imageSource;
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