using System;
using MonoTouch.UIKit;
using Spectator.Core.Model.Database;
using Spectator.Core.Model;
using Microsoft.Practices.ServiceLocation;

namespace Spectator.Ios
{
	partial class SnapshotViewCell : UITableViewCell
	{
		ImageModel model = ServiceLocator.Current.GetInstance<ImageModel> ();

		public SnapshotViewCell (IntPtr handle) : base (handle)
		{
		}

		public void Update (Snapshot snapshot)
		{
			Title.Text = snapshot.Title;
			Poster.SetImageSource (model.GetThumbnailUrl (snapshot.ThumbnailImageId, 300));
		}
	}
}