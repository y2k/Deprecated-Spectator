using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Spectator.WP8.ViewModel.Messages;

namespace Spectator.WP8.ViewModel.Base
{
    public class BaseViewModel : ViewModelBase
    {
        public virtual void OnStart() { }

        public virtual void OnStop() { }

        public void NavigateToViewModel<T>()
        {
            Messenger.Default.Send(new NavigationMessage { Target = typeof(T) });
        }

        public void NavigateBack()
        {
            Messenger.Default.Send(NavigationMessage.GoBack);
        }
    }
}