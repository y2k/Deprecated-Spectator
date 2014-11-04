using Spectator.Core.Model.Web.Proto;

namespace Spectator.Core.Model.Database
{
	static class ObjectExtensions
	{
		public static Snapshot ConvertToSnapshot (this SnapshotsResponse.ProtoSnapshot s, int subscriptionId)
		{
			return new Snapshot {
				SubscriptionId = subscriptionId,
				Title = s.Title,
				ThumbnailWidth = s.ThumbnailWidth,
				ThumbnailHeight = s.ThumbnailHeight,
				ThumbnailImageId = s.Thumbnail
			};
		}

		public static Subscription ConvertToSubscription (this SubscriptionResponse.ProtoSubscription s)
		{
			return new Subscription {
				ServerId = s.SubscriptionId,
				Title = s.Title,
				ThumbnailImageId = s.Thumbnail,
				GroupTitle = s.Group,
				UnreadCount = s.UnreadCount,
				Source = s.Source,
			};
		}
	}
}