using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;

namespace TSoft.TVServer.Entities
{
    /// <summary> An object item. </summary>
	[XmlRoot("items")]
	public class ObjectItem
	{
        #region [Public Properties]
        /// <summary> Gets or sets the recorded TV. </summary>
        /// <value> The recorded TV. </value>
        [XmlElement("recorded_tv")]
        public List<ObjectItemRecordedTV> RecordedTv { get; set; } = new List<ObjectItemRecordedTV>();

        /// <summary> Gets or sets the video. </summary>
        /// <value> The video. </value>
		[XmlElement("video")]
        public List<ObjectItemVideo> Video { get; set; } = new List<ObjectItemVideo>();

        #endregion

    }
}
