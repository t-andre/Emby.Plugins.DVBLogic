using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{
	/// <summary> Values that represent Enumeration schedule priorities. </summary>
	public enum EnumSchedulePriority
	{
		/// <summary> An enum constant representing the sp low option. </summary>
		[XmlEnum("-1")]
		SP_LOW = -1,

		/// <summary> An enum constant representing the sp normal option. </summary>
		[XmlEnum("0")]
		SP_NORMAL = 0,

		/// <summary> An enum constant representing the sp high option. </summary>
		[XmlEnum("1")]
		SP_HIGH = 1
	}
}
