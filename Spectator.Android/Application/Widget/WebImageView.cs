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

namespace Spectator.Android.Application.Widget
{
	public class WebImageView : ImageView
	{
		private IImageModel iModel = ServiceLocator.Current.GetInstance<IImageModel> ();

		private string imageSource;

		public string ImageSource {
			get { return imageSource; }
			set { UpdateImageSource (value); }
		}

		public WebImageView (Context context, global::Android.Util.IAttributeSet attrs) : base (context, attrs) { }

		private void UpdateImageSource (string imageSource) {
			if (this.imageSource != imageSource) {
				this.imageSource = imageSource;

				SetImageDrawable(null);

				var u = imageSource == null ? null : new Uri (imageSource); // u == null отменяет закачки
				iModel.Load (this, u, 0, s => {
					if (s == null || s.Image == null) SetImageDrawable(null);
					else SetImageBitmap ((Bitmap)s.Image);
				}); 
			}
		}
	}
}