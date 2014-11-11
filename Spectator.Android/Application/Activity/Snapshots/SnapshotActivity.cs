using Android.App;
using Android.Content;
using Android.OS;
using Spectator.Android.Application.Activity.Common.Base;

namespace Spectator.Android.Application.Activity.Snapshots
{
	[Activity (Label = "Snapshot")]
	public class SnapshotActivity : BaseActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			if (savedInstanceState == null)
				SetContentFragment (new ContentSnapshotFragment ());
		}

		public static Intent NewIntent (int snapshotId)
		{
			return new Intent (App.Current, typeof(SnapshotActivity)).PutExtra ("id", snapshotId);
		}
	}
}