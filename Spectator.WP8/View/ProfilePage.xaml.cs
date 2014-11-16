using Microsoft.Phone.Controls;
using Spectator.Core.Model.Account;
using System.Windows;
using System.Windows.Navigation;

namespace Spectator.WP8
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        Account model = new Account();

        public ProfilePage()
        {
            InitializeComponent();

            //Browser.Source = model.LoginStartUrl;
        }

        private async void Browser_Navigating(object sender, NavigatingEventArgs e)
        {
            //if (model.IsValid("" + e.Uri))
            //{
            //    e.Cancel = true;
            //    Progress.Visibility = Visibility.Visible;
            //    await model.LoginViaCodeAsync("" + e.Uri);

            //    //while (NavigationService.CanGoBack) NavigationService.GoBack();
            //    //NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
            //    NavigationService.GoBack();
            //}
            //else if (model.IsAccessDenied("" + e.Uri))
            //{
            //    NavigationService.GoBack();
            //    e.Cancel = true;
            //}
        }
    }
}