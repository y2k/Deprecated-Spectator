using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Spectator.Android.Application.Activity.Common.Base;

namespace Spectator.Android.Application.Activity
{
	[Activity (Label = "Snapshot")]			
	public class SnapshotActivity : BaseActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
		}
	}
}