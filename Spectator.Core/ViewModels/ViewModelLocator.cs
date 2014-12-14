using System;

namespace Spectator.Core.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            //ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            //SimpleIoc.Default.Register<MainViewModel>();
        }

        Lazy<SubscriptionsViewModel> _subscriptions = new Lazy<SubscriptionsViewModel>();
        public SubscriptionsViewModel Subscriptions { get { return _subscriptions.Value; } }

        Lazy<SnapshotsViewModel> _snapshots = new Lazy<SnapshotsViewModel>();
        public SnapshotsViewModel Snapshots { get { return _snapshots.Value; } }

        Lazy<CreateSubscriptionViewModel> _createSubscription = new Lazy<CreateSubscriptionViewModel>();
        public CreateSubscriptionViewModel CreateSubscription { get { return _createSubscription.Value; } }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}