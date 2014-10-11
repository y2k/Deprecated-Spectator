using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Spectator.Core.Model.Web.Proto
{
	[DataContract]
	public class ProtoSnapshotsResponse
	{
		[DataMember]
		public List<ProtoSnapshotsResponse.ProtoSnapshot> Snapshots { get; set; }

		public ProtoSnapshotsResponse ()
		{
			Snapshots = new List<ProtoSnapshotsResponse.ProtoSnapshot> ();
		}

		[DataContract]
		public class ProtoSnapshot
		{
			[DataMember]
			public bool HasContent { get; internal set; }

			[DataMember]
			public bool HasRevisions { get; internal set; }

			[DataMember]
			public bool HasScreenshots { get; internal set; }

			[DataMember]
			public int Id { get; internal set; }

			[DataMember]
			public List<string> Images { get; internal set; }

			[DataMember]
			public string Source { get; internal set; }

			[DataMember]
			public int SubscriptionIcon { get; internal set; }

			[DataMember]
			public int SubscriptionId { get; internal set; }

			[DataMember]
			public string SubscriptionName { get; internal set; }

			[DataMember]
			public int Thumbnail { get; internal set; }

			[DataMember]
			public int ThumbnailHeight { get; internal set; }

			[DataMember]
			public int ThumbnailWidth { get; internal set; }

			[DataMember]
			public string Title { get; internal set; }

			[DataMember]
			public long Updated { get; internal set; }
		}
	}

	[DataContract]
	public class ProtoSubscriptionResponse
	{
		public List<ProtoSubscription> Subscriptions { get; set; }

		public ProtoSubscriptionResponse ()
		{
			Subscriptions = new List<ProtoSubscription> ();
		}

		[DataContract]
		public class ProtoSubscription
		{
			[DataMember]
			public int Id { get; internal set; }

			[DataMember]
			public string Group { get; internal set; }

			[DataMember]
			public string Source { get; internal set; }

			[DataMember]
			public int SubscriptionId { get; internal set; }

			[DataMember]
			public int Thumbnail { get; internal set; }

			[DataMember]
			public string Title { get; internal set; }

			[DataMember]
			public int UnreadCount { get; internal set; }
		}
	}
}