using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.WP8.ViewModel.Base;
using System.Collections.ObjectModel;

namespace Spectator.WP8.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Subscription> Subscriptions { get; set; }

        private ISubscriptionModel model = ServiceLocator.Current.GetInstance<ISubscriptionModel>();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Subscriptions = new ObservableCollection<Subscription>();

            Initialize();

            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private async void Initialize()
        {
            Subscriptions.Clear();
            var t = await model.GetAllAsync();
            if (t.Value != null) foreach (var s in t.Value) Subscriptions.Add(s);
        }
    }
}