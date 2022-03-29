using System;
using System.ComponentModel;
using System.Net;
using System.Xml.Serialization;

namespace TSoft.TVServer.Entities
{
    /// <summary> A channel stream. </summary>
	[XmlRoot("stream")]
	public class ChannelStream
	{
		#region [Public Properties]
		[XmlElement(ElementName = "channel_handle")]
		public long ChannelHandle { get; set; }

        /// <summary> Gets or sets URL of the document. </summary>
        /// <value> The URL. </value>
		[XmlElement(ElementName = "url")]
		public string Url { get; set; }

		#endregion

	}
}
