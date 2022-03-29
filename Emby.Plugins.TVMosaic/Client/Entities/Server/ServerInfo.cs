using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;

namespace TSoft.TVServer.Entities
{
    /// <summary> Information about the server. </summary>
	[XmlRoot("server_info")]
	public class ServerInfo
	{
		#region [Public Properties]
        /// <summary> Gets or sets the identifier of the install. </summary>
        /// <value> The identifier of the install. </value>
		[XmlElement("install_id")]
		public string InstallId { get; set; }

        /// <summary> Gets or sets the identifier of the server. </summary>
        /// <value> The identifier of the server. </value>
		[XmlElement("server_id")]
		public string ServerId { get; set; }

        /// <summary> Gets or sets the version. </summary>
        /// <value> The version. </value>
		[XmlElement("version")]
		public string Version { get; set; }

        /// <summary> Gets or sets the build. </summary>
        /// <value> The build. </value>
		[XmlElement("build")]
		public string Build { get; set; }

		#endregion

	}
}
