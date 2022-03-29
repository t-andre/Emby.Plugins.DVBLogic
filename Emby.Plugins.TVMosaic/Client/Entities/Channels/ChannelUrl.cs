using System;
using System.Net;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;

namespace TSoft.TVServer.Entities
{
    [XmlRoot("stream_info")]
    public class ChannelUrl
    {
        #region [Public Properties]
        /// <summary> Gets or sets the items. </summary>
        /// <value> The items. </value>
        [XmlElement("channel")]
        public List<ChannelUrlStream> Items { get; set; } = new List<ChannelUrlStream>();

        #endregion

    }
}
