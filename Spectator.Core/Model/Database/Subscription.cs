using System;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

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
	}
}