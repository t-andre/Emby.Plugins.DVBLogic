using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> A schedule updater. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
	[XmlRoot("update_schedule")]
    public class ScheduleUpdater : IRequest
	{
		#region [Constructors]
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ScheduleUpdater class. </summary>
		public ScheduleUpdater()
		{
			RecordingsToKeep = 0;
			RecordSeriesAnytime = true;
			NewOnly = true;
		}

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ScheduleUpdater class. </summary>
        /// <param name="schedule_id"> Identifier for the schedule. </param>
		public ScheduleUpdater(string schedule_id)
			: base()
		{
			ScheduleID = schedule_id;
		}

		#endregion

		#region [Public Properties]
        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "update_schedule"; }
        }

        /// <summary> Gets or sets the identifier of the schedule. </summary>
        /// <value> The identifier of the schedule. </value>
		[XmlElement(ElementName = "schedule_id")]
		public string ScheduleID { get; set; }

        /// <summary> Gets or sets a value indicating whether the new only. </summary>
        /// <value> true if new only, false if not. </value>
		[XmlElement(ElementName = "new_only")]
		public bool NewOnly { get; set; }

        /// <summary> Gets or sets a value indicating whether the record series anytime. </summary>
        /// <value> true if record series anytime, false if not. </value>
		[XmlElement(ElementName = "record_series_anytime")]
		public bool RecordSeriesAnytime { get; set; }

        /// <summary> Gets or sets the recordings to keep. </summary>
        /// <value> The recordings to keep. </value>
		[XmlElement(ElementName = "recordings_to_keep")]
		public int RecordingsToKeep { get; set; }

        /// <summary> Gets or sets the margine before. </summary>
        /// <value> The margine before. </value>
        [XmlElement(ElementName = "margine_before")]
        public int? MargineBefore { get; set; }

        /// <summary> Gets or sets the margine after. </summary>
        /// <value> The margine after. </value>
        [XmlElement(ElementName = "margine_after")]
        public int MargineAfter { get; set; }
        #endregion

    }
}
