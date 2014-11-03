using System;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

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

		public bool HasWebContent { get; private set; }

		public bool HasRevisions { get; private set; }

		public int ThumbnailImageId { get; set; }

		public int ThumbnailWidth { get; set; }

		public int ThumbnailHeight { get; set; }

		public DateTime Created { get; set; }
	}
}