using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{
    /// <summary> Values that represent enumeration states. </summary>
	public enum EnumState
	{
		/// <summary> An enum constant representing the rtvs in progress option. </summary>
		[XmlEnum("0")]
		RTVS_IN_PROGRESS = 0,

		/// <summary> An enum constant representing the rtvs error option. </summary>
		[XmlEnum("1")]
		RTVS_ERROR = 1,

		/// <summary> An enum constant representing the rtvs forced to completion option. </summary>
		[XmlEnum("2")]
		RTVS_FORCED_TO_COMPLETION = 2,

		/// <summary> An enum constant representing the rtvs completed option. </summary>
		[XmlEnum("3")]
		RTVS_COMPLETED = 3
	}
}
