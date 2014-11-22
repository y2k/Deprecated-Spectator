using SQLite.Net.Attributes;

namespace Spectator.Core.Model.Database
{
	[Table ("cookies")]
	public class AccountCookie
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }
	}
}