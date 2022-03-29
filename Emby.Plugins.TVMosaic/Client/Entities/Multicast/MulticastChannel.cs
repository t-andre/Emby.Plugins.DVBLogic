using System;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;

namespace TSoft.TVServer.Entities
{
	/// <summary> A multicast channel. </summary>
	[XmlRoot("channel")]
	public class MulticastChannel
	{
		#region [Public Properties]
		/// <summary> Gets or sets the identifier. </summary>
		/// <value> The identifier. </value>
		[XmlElement(ElementName = "id")]
		public long Id { get; set; }

		/// <summary> Gets or sets URL of the document. </summary>
		/// <value> The URL. </value>
		[XmlElement(ElementName = "url")]
		public string Url { get; set; }

		#endregion

	}
}
