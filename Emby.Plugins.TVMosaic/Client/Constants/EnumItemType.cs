using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{
    /// <summary> Values that represent enumeration item types. </summary>
	public enum EnumItemType
	{
		/// <summary> An enum constant representing the item unknown option. </summary>
		[XmlEnum("-1")]
		ITEM_UNKNOWN = -1,

		/// <summary> An enum constant representing the item recorded TV option. </summary>
		[XmlEnum("0")]
		ITEM_RECORDED_TV = 0,

		/// <summary> An enum constant representing the item video option. </summary>
		[XmlEnum("1")]
		ITEM_VIDEO = 1,

		/// <summary> An enum constant representing the item audio option. </summary>
		[XmlEnum("2")]
		ITEM_AUDIO = 2,

		/// <summary> An enum constant representing the item image option. </summary>
		[XmlEnum("3")]
		ITEM_IMAGE = 3,

		/// <summary> An enum constant representing the item other option. </summary>
		[XmlEnum("4")]
		ITEM_OTHER = 4
	}
}
