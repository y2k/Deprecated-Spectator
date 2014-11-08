using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Microsoft.Practices.ServiceLocation;
using Spectator.Android.Application.Activity.Common.Base;
using Spectator.Core.Model.Account;

namespace Spectator.Android.Application.Activity.Profile
{
	[Activity (Label = "Profile")]			
	public class ProfileActivity : BaseActivity
	{
		//		IProfileModel model = ServiceLocator.Current.GetInstance<IProfileModel> ();
		GoogleUrlParser authUrlParser = new GoogleUrlParser ();
		Account account = new Account ();

		WebView webview;
		View progress;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_profile);
			webview = FindViewById<WebView> (Resource.Id.webview);
			progress = FindViewById (Resource.Id.progress);

			webview.Settings.JavaScriptEnabled = true;
			webview.Settings.LoadsImagesAutomatically = true;
			webview.SetWebViewClient (new AuthWebClient () { activity = this });
			webview.LoadUrl ("" + authUrlParser.LoginStartUrl);
		}

		async void Login (string url)
		{
			progress.Visibility = ViewStates.Visible;
			var code = authUrlParser.GetCode (url);
			account.LoginByCode (code);
			StartActivity (new Intent (this, typeof(MainActivity)).AddFlags (ActivityFlags.ClearTop));
			Finish ();
		}

		class AuthWebClient : WebViewClient
		{
			internal ProfileActivity activity;

			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				if (activity.authUrlParser.IsStateSuccess (url))
					activity.Login (url);
				else if (activity.authUrlParser.IsStateAccessDenied (url))
					activity.Finish ();
				else
					view.LoadUrl (url);
				return true;
			}
		}
	}
}