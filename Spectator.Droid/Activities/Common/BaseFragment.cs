using System;
using Android.Support.V4.App;
using GalaSoft.MvvmLight.Messaging;

namespace Spectator.Droid.Activitis.Common
{
    public class BaseFragment : Fragment
    {
        Action<EventHandler> onStart;
        Action<EventHandler> onStop;
        EventHandler handler;

        public IMessenger MessengerInstance { get { return Messenger.Default; } }

        protected void AddStartStopEvent(EventHandler handler, Action<EventHandler> onStart, Action<EventHandler> onStop)
        {
            this.onStart = onStart;
            this.onStop = onStop;
            this.handler = handler;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            onStart = onStop = null;
            Messenger.Default.Unregister(this);
        }

        public override void OnStop()
        {
            base.OnStop();
            if (handler != null)
                onStop(handler);
        }

        public override void OnStart()
        {
            base.OnStart();

            if (handler != null)
            {
                onStart(handler);
                handler(this, null);
            }
        }
    }
}