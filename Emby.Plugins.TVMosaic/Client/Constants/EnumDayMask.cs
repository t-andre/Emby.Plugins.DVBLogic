using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{
    /// <summary> A bitfield of flags for specifying enumeration day masks. </summary>
	[Flags]
	public enum EnumDayMask
	{
		/// <summary> A binary constant representing the day mask once flag. </summary>
		[XmlEnum("0")]
		DAY_MASK_ONCE = 0,

		/// <summary> A binary constant representing the day mask sun flag. </summary>
		[XmlEnum("1")]
		DAY_MASK_SUN = 1,

		/// <summary> A binary constant representing the day mask monitor flag. </summary>
		[XmlEnum("2")]
		DAY_MASK_MON = 2,

		/// <summary> A binary constant representing the day mask tue flag. </summary>
		[XmlEnum("4")]
		DAY_MASK_TUE = 4,

		/// <summary> A binary constant representing the day mask wed flag. </summary>
		[XmlEnum("8")]
		DAY_MASK_WED = 8,

		/// <summary> A binary constant representing the day mask thu flag. </summary>
		[XmlEnum("16")]
		DAY_MASK_THU = 16,

		/// <summary> A binary constant representing the day mask fri flag. </summary>
		[XmlEnum("32")]
		DAY_MASK_FRI = 32,

		/// <summary> A binary constant representing the day mask sat flag. </summary>
		[XmlEnum("64")]
		DAY_MASK_SAT = 64,

		/// <summary> A binary constant representing the day mask daily flag. </summary>
		[XmlEnum("62")]
		DAY_MASK_DAILY = 62,

        [XmlEnum("255")]
        DAY_MASK_WEEKLY = DAY_MASK_MON | DAY_MASK_TUE | DAY_MASK_WED | DAY_MASK_THU | DAY_MASK_FRI
    }
}
