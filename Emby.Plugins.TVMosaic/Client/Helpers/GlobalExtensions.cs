// <copyright file="GlobalExtensions.cs" >
// Copyright (c) 2017 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>12.09.2017</date>
// <summary>Implements the global extensions class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSoft.TVServer.Constants;

namespace TSoft.TVServer.Helpers
{
    /// <summary> A global extensions. </summary>
    public static class GlobalExtensions
    {
		#region [Public Methods]
		/// <summary> Gets stream type. </summary>
		/// <param name="streamType"> Type of the stream. </param>
		/// <returns> The stream type. </returns>
		public static string GetStreamType(EnumStreamType streamType)
		{
			switch (streamType)
			{
				case EnumStreamType.STREAM_RAW_HTTP:
					return "raw_http";
				case EnumStreamType.STREAM_RAW_HTTP_TIMESHIFT:
					return "raw_http_timeshift";
				case EnumStreamType.STREAM_RAW_UDP:
					return "raw_udp";
				case EnumStreamType.STREAM_HLS:
					return "hls";
				case EnumStreamType.STREAM_RTP:
					return "rtp";
				case EnumStreamType.STREAM_WEBM:
					return "webm";
				case EnumStreamType.STREAM_MP4:
					return "mp4";
				case EnumStreamType.STREAM_H264TS:
					return "h264ts";
				case EnumStreamType.STREAM_H264TS_HTTP_TIMESHIFT:
					return "h264ts_http_timeshift";
				case EnumStreamType.STREAM_NONE:
					return "none";
				default:
					return "none";
			}
		}

		/// <summary> Converts a value to the bits. </summary>
		/// <param name="value"> The value. </param>
		/// <returns> The given data converted to the bits. </returns>
		public static int ConvertToBits(int? value)
        {
            var r = value * 1000;
            return (int)r;
        }

        /// <summary> Converts a value to a kilo bits. </summary>
        /// <param name="value"> The value. </param>
        /// <returns> The given data converted to a kilo bits. </returns>
        public static int ConvertToKiloBits(int? value)
        {
            var r = value / 1000;
            return (int)r;
        }

        #endregion

    }
}
