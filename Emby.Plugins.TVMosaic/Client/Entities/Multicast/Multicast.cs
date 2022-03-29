using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;

namespace TSoft.TVServer.Entities
{
	/// <summary> A multicast. </summary>
	[XmlRoot("get_mcast_url_cmd")]
	public class Multicast
	{
        #region [Public Properties]
        /// <summary> Gets or sets the items. </summary>
        /// <value> The items. </value>
        [XmlElement("channel")]
        public List<MulticastChannel> Items { get; set; } = new List<MulticastChannel>();

        #endregion

    }
}
