using System;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Spectator.Core.Model.Database;
using Spectator.Core.Model;
using System.Collections.Generic;

namespace Spectator.Ios
{
	public partial class SnapshotListController : UITableViewController
	{
		IEnumerable<Snapshot> snapshots = new List<Snapshot> ();

		public SnapshotListController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
//			TableView.Hidden = true;
		}

		public void SetSubscriptionId (int subscriptionId)
		{
			ReloadSnapshots (subscriptionId);
		}

		async void ReloadSnapshots (int subscriptionId)
		{
			var model = new SnapshotCollectionModel (subscriptionId);
			await model.Reset ();
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			await model.Next ();
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			snapshots = await model.Get ();
			TableView.ReloadData ();
//			TableView.Hidden = false;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (SnapshotViewCell)tableView.DequeueReusableCell ("cell");
			cell.Update (snapshots.ElementAt (indexPath.Row));
			return cell;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return snapshots.Count ();
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			var snap = snapshots.ElementAt (indexPath.Row);
			float height = 46;
			if (snap.ThumbnailWidth > 0)
				height += (tableView.Bounds.Width / snap.ThumbnailWidth) * snap.ThumbnailHeight;
			return height;
		}
	}
}