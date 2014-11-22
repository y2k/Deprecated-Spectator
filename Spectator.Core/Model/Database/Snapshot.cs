using System;
using SQLite.Net.Attributes;

namespace Spectator.Core.Model.Database
{
	[Table ("snapshots")]
	public class Snapshot
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public int ServerId { get; set; }

		public int SubscriptionId { get; set; }

		public string Title { get; set; }

		public bool HasWebContent { get; set; }

		public bool HasRevisions { get; set; }

		public int ThumbnailImageId { get; set; }

		public int ThumbnailWidth { get; set; }

		public int ThumbnailHeight { get; set; }

		public DateTime Created { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Snapshot: Id={0}, ServerId={1}, SubscriptionId={2}, Title={3}, HasWebContent={4}, HasRevisions={5}, ThumbnailImageId={6}, ThumbnailWidth={7}, ThumbnailHeight={8}, Created={9}]", Id, ServerId, SubscriptionId, Title, HasWebContent, HasRevisions, ThumbnailImageId, ThumbnailWidth, ThumbnailHeight, Created);
		}

		public override bool Equals (object obj)
		{
			return ToString ().Equals ("" + obj);
		}

		public override int GetHashCode ()
		{
			return ToString ().GetHashCode ();
		}
	}
}