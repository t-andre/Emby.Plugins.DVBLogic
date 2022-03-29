using System;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;

namespace TSoft.TVServer.Entities
{
    /// <summary> A parental status. </summary>
	[XmlRoot("parental_status")]
	public class ParentalStatus
	{
		#region [Public Properties]

        /// <summary> Gets or sets a value indicating whether this ParentalStatus is enabled. </summary>
        /// <value> True if this ParentalStatus is enabled, false if not. </value>
		[XmlElement(ElementName = "is_enabled"), DefaultValue(false)]
		public bool IsEnabled { get; set; }

		#endregion

	}
}
