using Android.Webkit;
using GalaSoft.MvvmLight;

namespace Spectator.Droid.Activitis
{
	class AuthWebViewDecorator : ObservableObject
	{
		string _url;
		public string Url { get { return _url; } set { Set (ref _url, value); } }

		string _title;
		public string Title { get { return _title; } set { Set (ref _title, value); } }

		public AuthWebViewDecorator (WebView webview)
		{
			webview.Settings.SaveFormData = false;
			webview.Settings.JavaScriptEnabled = true;
			webview.Settings.LoadsImagesAutomatically = true;
			webview.SetWebViewClient (new WebViewClientImpl { parent = this });
			PropertyChanged += (sender, e) => { if (e.PropertyName == "Url") webview.LoadUrl(Url); };
		}

		class WebViewClientImpl : WebViewClient {

			internal AuthWebViewDecorator parent;

			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				view.LoadUrl (url);
				return true;
			}

			public override void OnPageFinished (WebView view, string url)
			{
				parent.Title = view.Title;
			}
		}
	}
}