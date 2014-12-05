using System;
using System.IO;

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

		public static void WriteAllStream (this Stream current, Stream other)
		{
			var buf = new byte[4 * 1024];
			int count;
			while ((count = other.Read (buf, 0, buf.Length)) > 0)
				current.Write (buf, 0, count);
		}
	}
}