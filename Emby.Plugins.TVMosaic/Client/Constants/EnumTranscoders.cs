using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{
    /// <summary> A bitfield of flags for specifying enumeration transcoders. </summary>
	[Flags]
	public enum EnumTranscoders
	{
		/// <summary> A binary constant representing the unsupported transcoders flag. </summary>
		[XmlEnum("0")]
		UNSUPPORTED_TRANSCODERS = 0,

		/// <summary> A binary constant representing the wmv type flag. </summary>
		[XmlEnum("1")]
        WMV_TYPE = 1,

		/// <summary> A binary constant representing the WMA type flag. </summary>
		[XmlEnum("2")]
        WMA_TYPE = 2,

		/// <summary> A binary constant representing the 264 type flag. </summary>
		[XmlEnum("4")]
        H264_TYPE = 4,

		/// <summary> A binary constant representing the aac type flag. </summary>
		[XmlEnum("8")]
        AAC_TYPE = 8,

		/// <summary> A binary constant representing the raw type flag. </summary>
		[XmlEnum("16")]
        RAW_TYPE = 16,

        /// <summary> A binary constant representing the vpx type flag. </summary>
		[XmlEnum("32")]
        VPX_TYPE = 32,

        /// <summary> A binary constant representing the vorbis type flag. </summary>
		[XmlEnum("64")]
        VORBIS_TYPE = 64,

		/// <summary> A binary constant representing all supported transcoders flag. </summary>
		[XmlEnum("65535")]
		ALL_SUPPORTED_TRANSCODERS = 65535
	}
}
