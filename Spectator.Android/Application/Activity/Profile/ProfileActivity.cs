
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
using Spectator.Core.Model;
using Microsoft.Practices.ServiceLocation;
using Android.Webkit;

namespace Spectator.Android.Application.Activity.Profile
{
	[Activity (Label = "Profile")]			
	public class ProfileActivity : BaseActivity
	{
		private IProfileModel model = ServiceLocator.Current.GetInstance<IProfileModel> ();

		private WebView webview;
		private View progress;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_profile);
			webview = FindViewById<WebView> (Resource.Id.webview);
			progress = FindViewById (Resource.Id.progress);

			webview.Settings.JavaScriptEnabled = true;
			webview.Settings.LoadsImagesAutomatically = true;
			webview.SetWebViewClient (new AuthWebClient () { activity = this });
			webview.LoadUrl("" + model.LoginStartUrl);
		}

		private async void Login(string url) {
			progress.Visibility = ViewStates.Visible;
			await model.LoginViaCodeAsync (url);
			StartActivity (new Intent (this, typeof(MainActivity)).AddFlags(ActivityFlags.ClearTop));
			Finish ();
		}

		private class AuthWebClient : WebViewClient {

			internal ProfileActivity activity;

			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				if (activity.model.IsValid (url)) activity.Login (url);
				else if (activity.model.IsAccessDenied (url)) activity.Finish ();
				else view.LoadUrl (url);
				return true;
			}
		}
	}
}