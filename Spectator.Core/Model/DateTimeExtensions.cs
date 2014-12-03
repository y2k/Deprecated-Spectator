using System;

namespace Spectator.Core.Model
{
	public static class DateTimeExtensions
	{
		public static DateTime UnixTimeStampToDateTime (this double unixTimeStamp)
		{
			var dtDateTime = new DateTime (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds (unixTimeStamp).ToLocalTime ();
			return dtDateTime;
		}

		public static DateTime MsUnixTimeStampToDateTime (this long msUnixTimeStamp)
		{
			return (msUnixTimeStamp / 1000.0).UnixTimeStampToDateTime ();
		}

		public static double DateTimeToUnixTimestamp (this DateTime dateTime)
		{
			return (dateTime - new DateTime (1970, 1, 1).ToLocalTime ()).TotalSeconds;
		}

		public static long DateTimeToMsUnixTimestamp (this DateTime dateTime)
		{
			return (long)(dateTime.DateTimeToUnixTimestamp () * 1000);
		}
	}
}