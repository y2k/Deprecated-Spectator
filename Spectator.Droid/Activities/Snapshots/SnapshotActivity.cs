using Android.App;
using Android.Content;
using Android.OS;
using Spectator.Droid.Activitis.Common;
using Spectator.Droid;

namespace Spectator.Droid.Activitis.Snapshots
{
	[Activity (Label = "@string/snapshot")]
	public class SnapshotActivity : BaseActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			if (savedInstanceState == null)
				SwitchToInfo ();
		}

		public static Intent NewIntent (int snapshotId)
		{
			return new Intent (App.Current, typeof(SnapshotActivity)).PutExtra ("id", snapshotId);
		}

		public void SwitchToWeb ()
		{
            // TODO:
//			SetContentFragment (new WebSnapshotFragment { Arguments = Intent.Extras });
		}

		public void SwitchToInfo ()
		{
            // TODO:
//			SetContentFragment (new ContentSnapshotFragment { Arguments = Intent.Extras });
		}
	}
}