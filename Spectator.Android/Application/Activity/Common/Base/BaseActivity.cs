using Android.Support.V4.App;
using Android.Support.V7.App;

namespace Spectator.Android.Application.Activity.Common.Base
{
	public class BaseActivity : ActionBarActivity
	{
		protected void SetContentFragment (Fragment fragment)
		{
			SupportFragmentManager.BeginTransaction ().Add (global::Android.Resource.Id.Content, fragment).Commit ();
		}
	}
}