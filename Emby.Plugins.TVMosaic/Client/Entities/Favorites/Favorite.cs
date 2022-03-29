using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Constants;

namespace TSoft.TVServer.Entities
{
	/// <summary> Information about the server. </summary>
	[XmlRoot("server_info")]
	public class Favorite
	{
		#region [Public Properties]

		[XmlElement("id")]
		public string Id { get; set; }

		/// <summary> Gets or sets the identifier of the server. </summary>
		/// <value> The identifier of the server. </value>
		[XmlElement("name")]
		public string Name { get; set; }

		/// <summary> Gets or sets the flags. </summary>
		/// <value> The flags. </value>
		[XmlElement("flags")]
		public EnumFavoriteFlags Flags { get; set; }

        /// <summary> Gets or sets the items. </summary>
        /// <value> The items. </value>
        [XmlArray("channels"), XmlArrayItem("channel")]
        public List<string> Channels { get; set; } = new List<string>();

        #endregion
    }
}
