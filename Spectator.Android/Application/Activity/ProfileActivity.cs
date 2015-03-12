using System;
using System.ComponentModel;
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
    [Activity(Label = "Profile", ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait)]			
    public class ProfileActivity : BaseActivity
    {
        LoginViewModel viewmodel = new LoginViewModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_profile);

            var progress = FindViewById(Resource.Id.progress);
            var client = new AuthWebClient(FindViewById<WebView>(Resource.Id.webview));
            viewmodel
                .SetBinding(() => viewmodel.IsBusy, progress, () => progress.Visibility)
                .ConvertSourceToTarget(s => s ? ViewStates.Visible : ViewStates.Gone);
            viewmodel.SetBinding(() => viewmodel.BrowserUrl, client, () => client.Url, BindingMode.TwoWay);

            MessengerInstance.Register<LoginViewModel.NavigateToHomeMessage>(this, _ => NavigateToHome());
        }

        void NavigateToHome()
        {
            StartActivity(new Intent(this, typeof(MainActivity)).AddFlags(ActivityFlags.ClearTop));
            Finish();
        }

        class AuthWebClient : WebViewClient , INotifyPropertyChanged
        {
            string _url;

            public string Url
            { 
                get { return _url; }
                set
                {
                    if (_url != value)
                    {
                        _url = value;
                        webview.LoadUrl(_url);
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Url"));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            WebView webview;

            public AuthWebClient(WebView webview)
            {
                this.webview = webview;
                webview.Settings.SaveFormData = false;
                webview.Settings.SavePassword = false;
                webview.Settings.JavaScriptEnabled = true;
                webview.Settings.LoadsImagesAutomatically = true;
                webview.SetWebViewClient(this);
            }

            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                view.LoadUrl(Url = url);
                return true;
            }
        }
    }
}