using System;
using System.Collections.Generic;

namespace Spectator.Droid.Activities.Common
{
	public class SelectSubscrptionCommand
	{
		static ISet<SelectSubscrptionCommand> ActiveCommands = new HashSet<SelectSubscrptionCommand> ();

		Action<int> callback;
		int subscriptionId;

		public SelectSubscrptionCommand (int subscriptionId)
		{
			this.subscriptionId = subscriptionId;
		}

		public SelectSubscrptionCommand (Action<int> action)
		{
			callback = action;
			ActiveCommands.Add (this);
		}

		public void Execute ()
		{
			foreach (var s in ActiveCommands)
				s.callback (subscriptionId);
		}

		public void Close ()
		{
			ActiveCommands.Remove (this);
		}
	}
}