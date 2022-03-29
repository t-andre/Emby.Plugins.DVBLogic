using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{
    /// <summary> Values that represent enumeration stream requests. </summary>
	public enum EnumStreamRequest
	{
        /// <summary> An enum constant representing the direct option. </summary>
		[XmlEnum("0")]
		DIRECT = 0,

        /// <summary> An enum constant representing the indirect option. </summary>
		[XmlEnum("1")]
		INDIRECT = 1,
	}
}
