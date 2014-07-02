using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Spectator.Android.Application.Activity.Common.Base;
using Spectator.Android.Application.Activity.Common.Commands;
using Android.Support.V4.Widget;

namespace Spectator.Android.Application.Activity
{
	[Activity (Label = "Spectator", MainLauncher = true)]
	public class MainActivity : BaseActivity
	{
		private SelectSubscrptionCommand openSubCommand;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
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