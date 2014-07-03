using System;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace Spectator.Core.Model.Database
{
	[Table ("snapshots")]
	public class Snapshot
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public int SubscriptionId { get; set; }

		public string Title { get; set; }

		public int ThumbnailImageId { get; set; }
	}
}