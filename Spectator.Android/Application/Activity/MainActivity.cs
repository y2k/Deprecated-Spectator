using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Spectator.Android.Application.Activity.Common.Base;
using Spectator.Android.Application.Activity.Common.Commands;

namespace Spectator.Android.Application.Activity
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