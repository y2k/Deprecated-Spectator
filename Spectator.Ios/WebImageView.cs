using System;
using MonoTouch.UIKit;
using Spectator.Core.Model;
using Microsoft.Practices.ServiceLocation;
using XamarinCommons.Image;

namespace Spectator.Ios
{
	partial class WebImageView : UIImageView
	{
		ImageModel model;
		string imageUrl;

		public WebImageView (IntPtr handle) : base (handle)
		{
		}

		public void SetImageSource (string imageUrl)
		{
			if (this.imageUrl != imageUrl) {
				this.imageUrl = imageUrl;
				Image = null;
				var link = imageUrl == null ? null : new Uri (imageUrl);

				if (model == null)
					model = ServiceLocator.Current.GetInstance<ImageModel> ();
				model.Load (this, link, 0, s => Image = ToImage (s));
			}
		}

		UIImage ToImage (ImageWrapper s)
		{
			return s == null || s.Image == null ? null : (UIImage)s.Image;
		}
	}
}