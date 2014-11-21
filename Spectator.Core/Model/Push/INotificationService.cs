using System;

namespace Spectator.Core.Model.Push
{
	public interface INotificationService
	{
		void ShowHasNewMessage (int updatedSubscriptions, int totalNewShapshots);
	}
}