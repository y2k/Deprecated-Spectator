using Android.App;
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
				FragmentManager
					.BeginTransaction ()
					.Add (global::Android.Resource.Id.Content, new ContentSnapshotFragment ())
					.Commit ();
		}
	}
}