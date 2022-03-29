using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{   
	/// <summary> Values that represent Enumeration favorite flags. </summary>
	public enum EnumFavoriteFlags
	{

		/// <summary> An enum constant representing the ff none option. </summary>
		[XmlEnum("0")]
		FF_NONE = 0,

		/// <summary> An enum constant representing the ff automatic option. </summary>
		[XmlEnum("1")]
		FF_AUTOMATIC = 1,

		/// <summary> An enum constant representing the ff user created option. </summary>
		[XmlEnum("2")]
		FF_USER_CREATED = 2
	}
}
