using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
	/// <summary> A multicast request. </summary>
	/// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
	[XmlRoot("get_mcast_url_cmd")]
	public class MulticastRequest : IRequest
	{
		#region [Private properties]

		/// <summary> Gets or sets the channel. </summary>
		/// <value> The channel. </value>
		[XmlElement(ElementName = "channel ")]
		public long Channel { get; set; }

		/// <summary> Gets the HTTP command. </summary>
		/// <value> The HTTP command. </value>
		/// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
		public string HttpCommand
		{
			get { return "get_mcast_url_cmd"; }
		}
		#endregion

	}
}
