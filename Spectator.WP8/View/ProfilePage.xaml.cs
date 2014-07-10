using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Spectator.Core.Model;
using Microsoft.Practices.ServiceLocation;

namespace Spectator.WP8
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        private IProfileModel model = ServiceLocator.Current.GetInstance<IProfileModel>();

        public ProfilePage()
        {
            InitializeComponent();

            Browser.Source = model.LoginStartUrl;
        }

        private async void Browser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (model.IsValid("" + e.Uri))
            {
                e.Cancel = true;
                Progress.Visibility = Visibility.Visible;
                await model.LoginViaCodeAsync("" + e.Uri);

                //while (NavigationService.CanGoBack) NavigationService.GoBack();
                //NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
                NavigationService.GoBack();
            }
            else if (model.IsAccessDenied("" + e.Uri))
            {
                NavigationService.GoBack();
                e.Cancel = true;
            }
        }
    }
}