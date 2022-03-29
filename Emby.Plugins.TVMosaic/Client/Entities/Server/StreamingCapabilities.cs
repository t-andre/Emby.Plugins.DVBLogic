using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Constants;

namespace TSoft.TVServer.Entities
{
    /// <summary> A streaming capabilities. </summary>
	[XmlRoot("streaming_caps")]
	public class StreamingCapabilities
	{
		#region [Public Properties]
        /// <summary> Gets or sets the protocols. </summary>
        /// <value> The protocols. </value>
		[XmlElement(ElementName = "protocols")]
		public int Protocols { get; set; }

        /// <summary> Gets or sets the transcoders. </summary>
        /// <value> The transcoders. </value>
		[XmlElement(ElementName = "transcoders")]
		public int Transcoders { get; set; }

        /// <summary> Gets or sets the pb transcoders. </summary>
        /// <value> The pb transcoders. </value>
		[XmlElement(ElementName = "pb_transcoders")]
		public int PbTranscoders { get; set; }

        /// <summary> Gets or sets the pb protocols. </summary>
        /// <value> The pb protocols. </value>
		[XmlElement(ElementName = "pb_protocols")]
		public int PbProtocols { get; set; }

        /// <summary> Gets or sets a value indicating whether we can record. </summary>
        /// <value> true if we can record, false if not. </value>
		[XmlElement(ElementName = "can_record"), DefaultValue(false)]
		public bool CanRecord { get; set; }

        /// <summary> Gets the sup pb protocols. </summary>
        /// <value> The sup pb protocols. </value>
		public EnumProtocols SupPbProtocols
		{
			get { return (EnumProtocols)(PbProtocols); }
		}

        /// <summary> Gets the sup pb transcoders. </summary>
        /// <value> The sup pb transcoders. </value>
		public EnumTranscoders SupPbTranscoders
		{
			get { return (EnumTranscoders)(PbTranscoders); }
		}

        /// <summary> Gets the sup protocols. </summary>
        /// <value> The sup protocols. </value>
		public EnumProtocols SupProtocols
		{
			get { return (EnumProtocols)(Protocols); }
		}

        /// <summary> Gets the sup transcoders. </summary>
        /// <value> The sup transcoders. </value>
		public EnumTranscoders SupTranscoders
		{
			get { return (EnumTranscoders)(Transcoders); }
		}

		#endregion

	}
}
