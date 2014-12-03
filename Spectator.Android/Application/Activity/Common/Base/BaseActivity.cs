using Android.Support.V4.App;
using Android.Support.V7.App;

namespace Spectator.Android.Application.Activity.Common.Base
{
	public class BaseActivity : ActionBarActivity
	{
		protected void SetContentFragment (Fragment fragment)
		{
			SetContentView (Resource.Layout.layout_container);
			SupportFragmentManager.BeginTransaction ().Add (Resource.Id.container, fragment).Commit ();
		}
	}
}