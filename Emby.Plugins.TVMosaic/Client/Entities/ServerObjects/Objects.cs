using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;

namespace TSoft.TVServer.Entities
{
    /// <summary> A server objects. </summary>
    [XmlRoot("object")]
    public class Objects
    {
        #region [Public Properties]
        /// <summary> Gets or sets the containers. </summary>
        /// <value> The containers. </value>
        [XmlArray("containers")]
        [XmlArrayItem("container")]
        public List<ObjectContainer> Containers { get; set; } = new List<ObjectContainer>();

        /// <summary> Gets or sets the items. </summary>
        /// <value> The items. </value>
        [XmlElement("items")]
        public ObjectItem Items { get; set; }

        /// <summary> Gets or sets the number of actuals. </summary>
        /// <value> The number of actuals. </value>
        [XmlElement(ElementName = "actual_count")]
        public int ActualCount { get; set; }

        /// <summary> Gets or sets the number of totals. </summary>
        /// <value> The total number of count. </value>
        [XmlElement(ElementName = "total_count")]
		public int TotalCount { get; set; }

        /// <summary> Gets or sets the nodes. </summary>
        /// <value> The nodes. </value>
        [XmlIgnore]
        public List<Objects> Nodes { get; set; }

        #endregion

    }
}
