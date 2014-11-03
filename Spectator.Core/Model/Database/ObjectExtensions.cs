using System;
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
	}
}