using System;
using Android.Support.V4.Content;
using System.Collections.Generic;

namespace Spectator.Android.Application.Activity.Common.Commands
{
	public class SelectSubscrptionCommand
	{
		private static ISet<SelectSubscrptionCommand> ActiveCommands = new HashSet<SelectSubscrptionCommand>();

		private Action<long> callback;
		private long subscriptionId;

		public SelectSubscrptionCommand (long subscriptionId)
		{
			this.subscriptionId = subscriptionId;
		}

		public SelectSubscrptionCommand (Action<long> action)
		{
			callback = action;
			ActiveCommands.Add (this);
		}

		public void Execute ()
		{
			foreach (var s in ActiveCommands) {
				s.callback (subscriptionId);
			}
		}

		public void Close ()
		{
			ActiveCommands.Remove (this);
		}
	}
}

