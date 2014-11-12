using Android.OS;
using Android.Views;
using Android.Widget;
using Spectator.Core.Model;
using Spectator.Android.Application.Activity.Common.Base;
using Spectator.Android.Application.Widget;

namespace Spectator.Android.Application.Activity.Snapshots
{
	public class ContentSnapshotFragment : BaseFragment
	{
		TextView title;
		TextView created;
		WebImageView image;

		SnapshotModel model;
		ImageModel imageModel = new ImageModel ();

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Resource.Layout.fragment_snapshot_content, null);
			title = v.FindViewById<TextView> (Resource.Id.title);
			created = v.FindViewById<TextView> (Resource.Id.created);
			image = v.FindViewById<WebImageView> (Resource.Id.image);
			return v;
		}

		public async override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RetainInstance = true;

			model = new SnapshotModel (Arguments.GetInt ("id"));
			await model.Reload ();
			var snapshot = await model.Get ();
			title.Text = snapshot.Title;
			created.Text = "" + snapshot.Created;
			image.ImageSource = imageModel.GetThumbnailUrl (snapshot.ThumbnailImageId, 200);
		}
	}
}