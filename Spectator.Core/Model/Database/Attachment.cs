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

		public override string ToString ()
		{
			return string.Format ("[Attachment: Width={0}, Height={1}, Image={2}]", Width, Height, Image);
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