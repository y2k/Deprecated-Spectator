using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Webkit;
using GalaSoft.MvvmLight.Helpers;
using Spectator.Android.Application.Activity.Common;
using Spectator.Core.ViewModels;

namespace Spectator.Android.Application.Activity
{
	[Activity (Label = "Profile", ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait)]			
	public class ProfileActivity : BaseActivity
	{
		LoginViewModel viewmodel = new LoginViewModel ();

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.activity_profile);

			var progress = FindViewById (Resource.Id.progress);
			var webview = new AuthWebViewDecorator (FindViewById<WebView> (Resource.Id.webview));
			viewmodel
                .SetBinding (() => viewmodel.IsBusy, progress, () => progress.Visibility)
                .ConvertSourceToTarget (s => s ? ViewStates.Visible : ViewStates.Gone);
			viewmodel.SetBinding (() => viewmodel.BrowserUrl, webview, () => webview.Url);
			webview.SetBinding (() => webview.Title, viewmodel, () => viewmodel.BrowserTitle);

			MessengerInstance.Register<LoginViewModel.NavigateToHomeMessage> (this, _ => NavigateToHome ());
		}

		void NavigateToHome ()
		{
			StartActivity (new Intent (this, typeof(MainActivity)).AddFlags (ActivityFlags.ClearTop));
			Finish ();
		}
	}
}