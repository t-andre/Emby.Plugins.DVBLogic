using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;
using TSoft.TVServer.Constants;

namespace TSoft.TVServer.Entities
{
    /// <summary> A by epg schedule. </summary>
	[XmlRoot("by_epg")]
	public class ByEpgSchedule
	{
		#region [Constructors]
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ByEpgSchedule class. </summary>
        /// <param name="channel_id"> Identifier for the channel. </param>
        /// <param name="program_id"> Identifier for the program. </param>
        /// <param name="is_repeat"> true if this TSoft.TVServer.Entities.ByEpgSchedule is repeat. </param>
		public ByEpgSchedule(string channel_id, string program_id, bool is_repeat)
		{
			ChannelID = channel_id;
			ProgramID = program_id;
			Repeat = is_repeat;
		}

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ByEpgSchedule class. </summary>
        /// <param name="channel_id"> Identifier for the channel. </param>
        /// <param name="program_id"> Identifier for the program. </param>
		public ByEpgSchedule(string channel_id, string program_id)
			: this(channel_id, program_id, false)
		{
		}

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ByEpgSchedule class. </summary>
		public ByEpgSchedule()
		{
		}

		#endregion

		#region [Public Properties]
        /// <summary> Gets or sets the identifier of the channel. </summary>
        /// <value> The identifier of the channel. </value>
		[XmlElement(ElementName = "channel_id")]
		public string ChannelID { get; set; }

        /// <summary> Gets or sets the identifier of the program. </summary>
        /// <value> The identifier of the program. </value>
		[XmlElement(ElementName = "program_id")]
		public string ProgramID { get; set; }

        /// <summary> Gets or sets a value indicating whether the repeat. </summary>
        /// <value> true if repeat, false if not. </value>
		[XmlElement(ElementName = "repeat")]
		public bool Repeat { get; set; }

        /// <summary> Gets or sets a value indicating whether the new only. </summary>
        /// <value> true if new only, false if not. </value>
		[XmlElement(ElementName = "new_only")]
		public bool NewOnly { get; set; }

		/// <summary> Gets or sets the day mask. </summary>
		/// <value> The day mask. </value>
		[XmlElement(ElementName = "day_mask")]
		public EnumDayMask DayMask { get; set; }

		/// <summary> Gets or sets the start before. </summary>
		/// <value> The start before. </value>
		[XmlElement(ElementName = "start_before")]
		public int StartBefore { get; set; } = -1;

		/// <summary> Gets or sets the start after. </summary>
		/// <value> The start after. </value>
		[XmlElement(ElementName = "start_after")]
		public int StartAfter { get; set; } = -1;

		/// <summary> Gets or sets a value indicating whether the record series anytime. </summary>
		/// <value> true if record series anytime, false if not. </value>
		[XmlElement(ElementName = "record_series_anytime"), DefaultValue(false)]
		public bool RecordSeriesAnytime { get; set; }

        /// <summary> Gets or sets the recordings to keep. </summary>
        /// <value> The recordings to keep. </value>
		[XmlElement(ElementName = "recordings_to_keep")]
		public int RecordingsToKeep { get; set; }

        /// <summary> Gets or sets the program. </summary>
        /// <value> The program. </value>
		[XmlElement(ElementName = "program")]
		public Program Program { get; set; }

		#endregion

	}
}
