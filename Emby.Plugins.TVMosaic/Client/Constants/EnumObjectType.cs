using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{
    /// <summary> Values that represent enumeration object types. </summary>
	public enum EnumObjectType
	{
		/// <summary> An enum constant representing the object unknown option. </summary>
		[XmlEnum("-1")]
		OBJECT_UNKNOWN = -1,

		/// <summary> An enum constant representing the object container option. </summary>
		[XmlEnum("0")]
		OBJECT_CONTAINER = 0,

		/// <summary> An enum constant representing the object item option. </summary>
		[XmlEnum("1")]
		OBJECT_ITEM = 1
	}
}
