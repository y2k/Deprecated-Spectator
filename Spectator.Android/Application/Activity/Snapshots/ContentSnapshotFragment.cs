using Android.OS;
using Android.Views;
using Spectator.Android.Application.Activity.Common.Base;

namespace Spectator.Android.Application.Activity.Snapshots
{
	public class ContentSnapshotFragment : BaseFragment
	{
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Resource.Layout.fragment_snapshot_content, null);
			return v;
		}
	}
}