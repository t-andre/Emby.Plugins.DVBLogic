using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;

namespace TSoft.TVServer.Entities
{
	/// <summary> A recording. </summary>
	[XmlRoot("recording")]
	public class Recording
	{
		#region [Public Properties]
		/// <summary> Gets or sets the identifier of the recording. </summary>
		/// <value> The identifier of the recording. </value>
		[XmlElement(ElementName = "recording_id")]
		public string RecordingID { get; set; }

		/// <summary> Gets or sets the identifier of the schedule. </summary>
		/// <value> The identifier of the schedule. </value>
		[XmlElement(ElementName = "schedule_id")]
		public string ScheduleID { get; set; }

		/// <summary> Gets or sets the identifier of the channel. </summary>
		/// <value> The identifier of the channel. </value>
		[XmlElement(ElementName = "channel_id")]
		public string ChannelID { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this TSoft.TVServer.Entities.Recording is
		/// active. </summary>
		/// <value>
		/// true if this TSoft.TVServer.Entities.Recording is active, false if not. </value>
		[XmlElement(ElementName = "is_active"), DefaultValue(false)]
		public bool IsActive { get; set; }

		/// <summary> Gets or sets the program. </summary>
		/// <value> The program. </value>
		[XmlElement(ElementName = "program")]
		public Program Program { get; set; }

		#endregion

	}
}
