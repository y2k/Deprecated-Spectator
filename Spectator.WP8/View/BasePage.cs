using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Spectator.WP8.ViewModel.Base;
using Spectator.WP8.ViewModel.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Spectator.WP8.View
{
    public class BasePage : PhoneApplicationPage
    {
        /// <summary>
        /// Вход на страницу
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Messenger.Default.Register<NavigationMessage>(this, s =>
                NavigationService.Navigate(new Uri("/View/" + s.Target.Name.Replace("ViewModel", "Page.xaml"), UriKind.Relative)));

            var vm = DataContext as BaseViewModel;
            if (vm != null) vm.OnStart();
        }

        /// <summary>
        /// Выход со страницы
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            var vm = DataContext as BaseViewModel;
            if (vm != null) vm.OnStop();

            Messenger.Default.Unregister(this);
        }
    }
}
