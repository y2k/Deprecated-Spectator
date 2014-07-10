using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Spectator.WP8.ViewModel.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Spectator.WP8.ViewModel.Base
{
    public class BaseViewModel : ViewModelBase
    {
        public virtual void OnStart() { }

        public virtual void OnStop() { }

        protected void NavigateToViewModel<T>()
        {
            Messenger.Default.Send(new NavigationMessage { Target = typeof(T) });
        }
    }
}