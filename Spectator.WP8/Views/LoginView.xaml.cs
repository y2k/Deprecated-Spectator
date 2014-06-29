using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Spectator.Core.ViewModels;

namespace Spectator.WP8.Views
{
    public partial class LoginView : MvxPhonePage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            ((LoginViewModel)DataContext).Initialize(new WebViewWrapper(WebBrowser));
        }

        private class WebViewWrapper : LoginViewModel.ICommonWebView
        {
            private WebBrowser webBrowser;

            public WebViewWrapper(WebBrowser webBrowser)
            {
                this.webBrowser = webBrowser;
            }

            public void OpenUrl(Uri url)
            {
                webBrowser.Source = url;
            }
        }
    }
}