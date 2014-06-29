using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Spectator.Android.Application.Activity.Common.Base;

namespace Spectator.Android.Application.Activity
{
	[Activity (Label = "Spectator", MainLauncher = true)]
	public class MainActivity : BaseActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.activity_main);
		}
	}
}