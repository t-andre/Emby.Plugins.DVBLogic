using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;

namespace TSoft.TVServer.Entities
{
    /// <summary> A recordings. </summary>
	[XmlRoot("recordings")]
	public class Recordings
	{
        #region [Public Properties]
        /// <summary> Gets or sets the items. </summary>
        /// <value> The items. </value>
        [XmlElement("recording")]
        public List<Recording> Items { get; set; } = new List<Recording>();

        #endregion

    }
}
