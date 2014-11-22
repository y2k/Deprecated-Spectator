using SQLite.Net.Attributes;

namespace Spectator.Core.Model.Database
{
	[Table ("subscriptions")]
	public class Subscription
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Title { get; set; }

		public int ThumbnailImageId { get; set; }

		public long ServerId { get; set; }

		public string GroupTitle { get; set; }

		public string Source { get; set; }

		public int UnreadCount { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Subscription: Id={0}, Title={1}, ThumbnailImageId={2}, ServerId={3}, GroupTitle={4}, Source={5}, UnreadCount={6}]", Id, Title, ThumbnailImageId, ServerId, GroupTitle, Source, UnreadCount);
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