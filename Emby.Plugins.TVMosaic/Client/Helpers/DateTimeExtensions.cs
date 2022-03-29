using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSoft.TVServer.Helpers
{
	/// <summary> A date time extensions. </summary>
	public static class DateTimeExtensions
	{
		#region [Fields]

		/// <summary> The unix epoch. </summary>
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		/// <summary> The unix epoch offset. </summary>
		private static readonly DateTimeOffset UnixEpochOffset = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
		#endregion

		#region [Public methods]

		/// <summary> Date time from unix timestamp millis. </summary>
		/// <param name="millis"> The millis. </param>
		/// <returns> A DateTime. </returns>
		public static DateTime DateTimeFromUnixTimestampMillis(this long millis)
		{
			return UnixEpoch.AddMilliseconds(millis);
		}

		/// <summary> A long extension method that date time offset from unix timestamp millis. </summary>
		/// <param name="millis"> The millis. </param>
		/// <returns> A DateTimeOffset. </returns>
		public static DateTimeOffset DateTimeOffsetFromUnixTimestampMillis(this long millis)
		{
			return UnixEpochOffset.AddMilliseconds(millis);
		}

		/// <summary> Date time from unix timestamp seconds. </summary>
		/// <param name="seconds"> The seconds. </param>
		/// <returns> A DateTime. </returns>
		public static DateTime DateTimeFromUnixTimestampSeconds(this long seconds)
		{
			return UnixEpoch.AddSeconds(seconds);
		}

		/// <summary> A long extension method that date time from unix timestamp offset seconds. </summary>
		/// <param name="seconds"> The seconds. </param>
		/// <returns> A DateTimeOffset. </returns>
		public static DateTimeOffset DateTimeFromUnixTimestampOffsetSeconds(this long seconds)
		{
			return UnixEpochOffset.AddSeconds(seconds);
		}

		/// <summary> Gets current unix timestamp millis. </summary>
		/// <returns> The current unix timestamp millis. </returns>
		public static long GetCurrentUnixTimestampMillis()
		{
			return (long)(DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
		}

		/// <summary> Gets current unix timestamp offset millis. </summary>
		/// <returns> The current unix timestamp offset millis. </returns>
		public static long GetCurrentUnixTimestampOffsetMillis()
		{
			return (long)(DateTimeOffset.UtcNow - UnixEpochOffset).TotalMilliseconds;
		}

		/// <summary> Gets current unix timestamp seconds. </summary>
		/// <param name="date"> The date Date/Time. </param>
		/// <returns> The current unix timestamp seconds. </returns>
		public static long GetCurrentUnixTimestampSeconds(this DateTime date)
		{
			return (long)(date - UnixEpoch).TotalSeconds;
		}

		/// <summary> A DateTimeOffset extension method that gets current unix timestamp offset seconds. </summary>
		/// <param name="date"> The date Date/Time. </param>
		/// <returns> The current unix timestamp offset seconds. </returns>
		public static long GetCurrentUnixTimestampOffsetSeconds(this DateTimeOffset date)
		{
			return (long)(date - UnixEpochOffset).TotalSeconds;
		}
		#endregion

	}
}
