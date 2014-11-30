using System;
using Spectator.Core.Model;

namespace Spectator.Core.Controllers
{
	public class CreateSubscriptionController
	{
		public bool TitleError { get; private set; }

		public bool LinkError { get; private set; }

		public bool InProgress { get; private set; }

		public string Title { get; set; }

		public string Link { get; set; }

		public Action CallbackUpdateUi { get; set; }

		public Action CallbackFinishSuccess { get; set; }

		public void OnClickedCreateSubscriptions ()
		{
			if (ValidCreateData ())
				CreateSubscription ();
		}

		bool ValidCreateData ()
		{
			LinkError = !Uri.IsWellFormedUriString (Link, UriKind.Absolute);
			TitleError = string.IsNullOrWhiteSpace (Title);
			UpdateUi ();
			return !LinkError && !TitleError;
		}

		async void CreateSubscription ()
		{
			SetProgressEnabled (true);
			try {
				await new SubscriptionModel ().CreateNew (new Uri (Link), Title);
				CallbackFinishSuccess ();
			} catch {
			}
			SetProgressEnabled (false);
		}

		void SetProgressEnabled (bool inProgress)
		{
			InProgress = inProgress;
			UpdateUi ();
		}

		void UpdateUi ()
		{
			CallbackUpdateUi ();
		}
	}
}