using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{
    /// <summary> Values that represent enumeration container types. </summary>
	public enum EnumContainerType
	{
		/// <summary> An enum constant representing the container unknown option. </summary>
		[XmlEnum("-1")]
		CONTAINER_UNKNOWN = -1,

		/// <summary> An enum constant representing the container source option. </summary>
		[XmlEnum("0")]
		CONTAINER_SOURCE = 0,

		/// <summary> An enum constant representing the container type option. </summary>
		[XmlEnum("1")]
		CONTAINER_TYPE = 1,

		/// <summary> An enum constant representing the container category option. </summary>
		[XmlEnum("2")]
		CONTAINER_CATEGORY = 2,

		/// <summary> An enum constant representing the container group option. </summary>
		[XmlEnum("3")]
		CONTAINER_GROUP = 3,

		/// <summary> An enum constant representing the container other 1 option. </summary>
		[XmlEnum("4")]
		CONTAINER_OTHER1 = 4,

		/// <summary> An enum constant representing the container other 2 option. </summary>
		[XmlEnum("5")]
		CONTAINER_OTHER2 = 5
	}
}
