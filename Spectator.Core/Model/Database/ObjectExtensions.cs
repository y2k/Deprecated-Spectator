using Spectator.Core.Model.Web;

namespace Spectator.Core.Model.Database
{
	static class ObjectExtensions
	{
		public static Snapshot ConvertToSnapshot (this SnapshotsResponse.ProtoSnapshot s)
		{
			return DoConvert (s, 0);
		}

		public static Snapshot ConvertToSnapshot (this SnapshotsResponse.ProtoSnapshot s, int subscriptionId)
		{
			return DoConvert (s, subscriptionId);
		}

		static Snapshot DoConvert (SnapshotsResponse.ProtoSnapshot s, int subscriptionId)
		{
			return new Snapshot {
				SubscriptionId = subscriptionId,
				Title = s.Title,
				ThumbnailWidth = s.ThumbnailWidth,
				ThumbnailHeight = s.ThumbnailHeight,
				ThumbnailImageId = s.Thumbnail,
				HasWebContent = s.HasContent,
				HasRevisions = s.HasRevisions,
				ServerId = s.Id,
				Created = s.Updated.MsUnixTimeStampToDateTime (),
				Source = s.Source,
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