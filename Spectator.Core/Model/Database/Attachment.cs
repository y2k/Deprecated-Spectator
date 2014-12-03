using SQLite.Net.Attributes;

namespace Spectator.Core.Model.Database
{
	[Table ("attachments")]
	public class Attachment
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public string Image { get; set; }

		public int SnapshotId { get; set; }

		public override string ToString ()
		{
			return string.Format (
				"[Attachment: Id={0}, Width={1}, Height={2}, Image={3}, SnapshotId={4}]",
				Id, Width, Height, Image, SnapshotId);
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