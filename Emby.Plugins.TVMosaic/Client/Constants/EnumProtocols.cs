using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{
    /// <summary> A bitfield of flags for specifying enumeration protocols. </summary>
	[Flags]
	public enum EnumProtocols
	{
		/// <summary> A binary constant representing the unsupported protocols flag. </summary>
		[XmlEnum("0")]
		UNSUPPORTED_PROTOCOLS = 0,

		/// <summary> A binary constant representing the HTTP type flag. </summary>
		[XmlEnum("1")]
        HTTP_TYPE = 1,

		/// <summary> A binary constant representing the UDP type flag. </summary>
		[XmlEnum("2")]
        UDP_TYPE = 2,

		/// <summary> A binary constant representing the rtsp type flag. </summary>
		[XmlEnum("4")]
        RTSP_TYPE = 4,

		/// <summary> A binary constant representing the asf type flag. </summary>
		[XmlEnum("8")]
        ASF_TYPE = 8,

		/// <summary> A binary constant representing the HLS type flag. </summary>
		[XmlEnum("16")]
        HLS_TYPE = 16,

		/// <summary> A binary constant representing the webm type flag. </summary>
		[XmlEnum("32")]
        WEBM_TYPE = 32,

        /// <summary> A binary constant representing the mp 4 type flag. </summary>
		[XmlEnum("64")]
        MP4_TYPE = 64,  

		/// <summary> A binary constant representing all supported protocols flag. </summary>
		[XmlEnum("65535")]
		ALL_SUPPORTED_PROTOCOLS = 65535
	}
}
