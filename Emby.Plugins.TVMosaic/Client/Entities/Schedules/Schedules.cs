using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;

namespace TSoft.TVServer.Entities
{
	/// <summary> A schedules. </summary>
	[XmlRoot("schedules")]
	public class Schedules
	{
        #region [Public Properties]

        /// <summary> Gets or sets the items. </summary>
        /// <value> The items. </value>
        [XmlElement("schedule")]
        public List<Schedule> Items { get; set; } = new List<Schedule>();

        #endregion

    }
}
