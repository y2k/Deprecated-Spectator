using System;
using Android.Support.V4.Content;

namespace Spectator.Android.Application.Activity.Common.Commands
{
	public class SelectSubscrptionCommand
	{
		private static SelectSubscrptionCommand ActiveCommand;

		private Action<long> callback;
		private long subscriptionId;

		public SelectSubscrptionCommand (long subscriptionId)
		{
			this.subscriptionId = subscriptionId;
		}

		public SelectSubscrptionCommand (Action<long> action)
		{
			callback = action;
			ActiveCommand = this;
		}

		public void Execute ()
		{
			if (ActiveCommand != null) ActiveCommand.callback (subscriptionId);
		}

		public void Close ()
		{
			ActiveCommand = null;
		}
	}
}

