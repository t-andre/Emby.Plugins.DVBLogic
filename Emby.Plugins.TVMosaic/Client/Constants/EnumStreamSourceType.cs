using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{
	/// <summary> Values that represent enumeration stream source types. </summary>
	public enum EnumStreamSourceType
	{
		/// <summary> An enum constant representing the none option. </summary>
		[XmlEnum("0")]
		NONE = 0,

		/// <summary> An enum constant representing the livetv option. </summary>
		[XmlEnum("1")]
		LIVETV = 1,

		/// <summary> An enum constant representing the recording option. </summary>
		[XmlEnum("2")]
		RECORDING = 2,

		/// <summary> An enum constant representing the other option. </summary>
		[XmlEnum("9")]
		OTHER = 9,
	}
}
