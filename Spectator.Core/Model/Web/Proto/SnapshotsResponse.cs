using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Spectator.Core.Model.Web.Proto
{
	[DataContract]
	public class SnapshotsResponse
	{
		[DataMember]
		public List<SnapshotsResponse.ProtoSnapshot> Snapshots { get; set; }

		public SnapshotsResponse ()
		{
			Snapshots = new List<SnapshotsResponse.ProtoSnapshot> ();
		}

		[DataContract]
		public class ProtoSnapshot
		{
			[DataMember]
			public bool HasContent { get; set; }

			[DataMember]
			public bool HasRevisions { get; set; }

			[DataMember]
			public bool HasScreenshots { get; set; }

			[DataMember]
			public int Id { get; set; }

			[DataMember]
			public List<string> Images { get; set; }

			[DataMember]
			public string Source { get; set; }

			[DataMember]
			public int SubscriptionIcon { get; set; }

			[DataMember]
			public int SubscriptionId { get; set; }

			[DataMember]
			public string SubscriptionName { get; set; }

			[DataMember]
			public int Thumbnail { get; set; }

			[DataMember]
			public int ThumbnailHeight { get; set; }

			[DataMember]
			public int ThumbnailWidth { get; set; }

			[DataMember]
			public string Title { get; set; }

			[DataMember]
			public long Updated { get; set; }
		}
	}

	[DataContract]
	public class ProtoSubscriptionResponse
	{
		[DataMember]
		public List<ProtoSubscription> Subscriptions { get; set; }

		public ProtoSubscriptionResponse ()
		{
			Subscriptions = new List<ProtoSubscription> ();
		}

		[DataContract]
		public class ProtoSubscription
		{
			[DataMember]
			public int Id { get; set; }

			[DataMember]
			public string Group { get; set; }

			[DataMember]
			public string Source { get; set; }

			[DataMember]
			public int SubscriptionId { get; set; }

			[DataMember]
			public int Thumbnail { get; set; }

			[DataMember]
			public string Title { get; set; }

			[DataMember]
			public int UnreadCount { get; set; }
		}
	}
}