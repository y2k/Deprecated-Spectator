using Android.Support.V4.App;
using Android.Support.V7.App;
using GalaSoft.MvvmLight.Messaging;
using Spectator.Droid;

namespace Spectator.Droid.Activities.Common
{
    public class BaseActivity : ActionBarActivity
    {
        public IMessenger MessengerInstance
        {
            get { return Messenger.Default; }
        }

        protected void SetContentFragment(Fragment fragment)
        {
            SetContentView(Resource.Layout.layout_container);
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, fragment).Commit();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Messenger.Default.Unregister(this);
        }
    }
}