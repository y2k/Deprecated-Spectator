using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Spectator.Droid.Activitis.Common;
using Spectator.Droid;

namespace Spectator.Droid.Activitis
{
	[Activity (Label = "Spectator", MainLauncher = true)]
	public class MainActivity : BaseActivity
	{
		SelectSubscrptionCommand openSubCommand;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.activity_main);

			var slidePanel = FindViewById<SlidingPaneLayout> (Resource.Id.slidePanel);
			openSubCommand = new SelectSubscrptionCommand (id => slidePanel.ClosePane ());
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			openSubCommand.Close ();
		}
	}
}