using Android.App;

namespace Spectator.Android.Application.Activity.Common.Base
{
	public class BaseActivity : global::Android.App.Activity
	{
		protected void SetContentFragment (Fragment fragment)
		{
			FragmentManager.BeginTransaction ().Add (global::Android.Resource.Id.Content, fragment).Commit ();
		}
	}
}