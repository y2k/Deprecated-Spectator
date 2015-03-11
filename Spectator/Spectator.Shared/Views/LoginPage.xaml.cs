using GalaSoft.MvvmLight.Messaging;
using Spectator.Core.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Spectator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Messenger.Default.Register<LoginViewModel.NavigateToHomeMessage>(
                this, _ => Frame.Navigate(typeof(MainPage)));
            BindWebViewSource();
        }

        void BindWebViewSource()
        {
            WebView.Source = new Uri((DataContext as LoginViewModel).BrowserUrl);
            WebView.NavigationStarting += (sender, ne) => (DataContext as LoginViewModel).BrowserUrl = "" + ne.Uri;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Messenger.Default.Unregister(this);
        }
    }
}