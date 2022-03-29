using System;
using System.ComponentModel;
using System.Net;
using System.Xml.Serialization;

namespace TSoft.TVServer.Entities
{
	/// <summary> A channel stream. </summary>
	[XmlRoot("channel")]
	public class ChannelUrlStream
	{
		#region [Public Properties]
		/// <summary> Gets or sets the identifier of the channel dvb link. </summary>
		/// <value> The identifier of the channel dvb link. </value>
		[XmlElement(ElementName = "channel_dvblink_id")]
		public string ChannelDVBLinkID { get; set; }

		/// <summary> Gets or sets URL of the document. </summary>
		/// <value> The URL. </value>
		[XmlElement(ElementName = "url")]
		public string Url { get; set; }

		#endregion

	}
}
