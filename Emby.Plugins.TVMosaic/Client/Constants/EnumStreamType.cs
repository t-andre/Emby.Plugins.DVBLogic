using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{
    /// <summary> Values that represent enumeration stream types. </summary>
	public enum EnumStreamType : int
	{

		/// <summary> An enum constant representing the stream raw HTTP option. </summary>
		[XmlEnum("raw_http")]
        STREAM_RAW_HTTP = 0,

		/// <summary> An enum constant representing the stream HTTP timeshift option. </summary>
		[XmlEnum("raw_http_timeshift")]
        STREAM_RAW_HTTP_TIMESHIFT = 1,

		/// <summary> An enum constant representing the stream raw UDP option. </summary>
		[XmlEnum("raw_udp")]
        STREAM_RAW_UDP = 2,

		/// <summary> An enum constant representing the stream HLS option. 
		/// 		  - IPhone</summary>
		[XmlEnum("hls")]
        STREAM_HLS = 3,

		/// <summary> An enum constant representing the stream rtp option. 
		/// 		  - Android</summary>
		[XmlEnum("rtp")]
        STREAM_RTP = 4,

		/// <summary> An enum constant representing the stream webm option. </summary>
		[XmlEnum("webm")]
        STREAM_WEBM = 5,

		/// <summary> An enum constant representing the stream mp 4 option. </summary>
		[XmlEnum("mp4")]
        STREAM_MP4 = 6,

		/// <summary> An enum constant representing the stream h 264 ts option. </summary>
		[XmlEnum("h264ts")]
        STREAM_H264TS = 7,

		/// <summary> An enum constant representing the stream h 264 ts HTTP timeshift option. </summary>
		[XmlEnum("h264ts_http_timeshift")]
        STREAM_H264TS_HTTP_TIMESHIFT = 8,

		/// <summary> An enum constant representing the stream asf option. 
		/// 		  - Windows Phone</summary>
		[XmlEnum("asf")]
        STREAM_ASF = 9,

        /// <summary> An enum constant representing the stream HTTP option. </summary>
		[XmlEnum("none")]
		STREAM_NONE = 100,
	}
}
