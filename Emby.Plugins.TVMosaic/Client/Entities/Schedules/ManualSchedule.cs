using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;
using TSoft.TVServer.Constants;

namespace TSoft.TVServer.Entities
{
    /// <summary> A manual schedule. </summary>
	[XmlRoot("manual")]
	public class ManualSchedule
	{
		#region [Constructors]
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ManualSchedule class. </summary>
		public ManualSchedule()
		{
		}

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ManualSchedule class. </summary>
        /// <param name="channel_id"> Identifier for the channel. </param>
        /// <param name="title"> The title. </param>
        /// <param name="start_time"> The start time. </param>
        /// <param name="duration"> The duration. </param>
        /// <param name="day_mask"> The day mask. </param>
		public ManualSchedule(string channel_id, string title, long start_time, int duration, EnumDayMask day_mask)
		{
			ChannelID = channel_id;
			Title = title;
			StartTime = start_time;
			Duration = duration;
			DayMask = day_mask;
		}

		#endregion

		#region [Public Properties]
        /// <summary> Gets or sets the identifier of the channel. </summary>
        /// <value> The identifier of the channel. </value>
		[XmlElement(ElementName = "channel_id")]
		public string ChannelID { get; set; }

        /// <summary> Gets or sets the title. </summary>
        /// <value> The title. </value>
		[XmlElement(ElementName = "title")]
		public string Title { get; set; }

        /// <summary> Gets or sets the start time. </summary>
        /// <value> The start time. </value>
		[XmlElement(ElementName = "start_time")]
		public long StartTime { get; set; }

        /// <summary> Gets or sets the duration. </summary>
        /// <value> The duration. </value>
		[XmlElement(ElementName = "duration")]
		public int Duration { get; set; }

        /// <summary> Gets or sets the day mask. </summary>
        /// <value> The day mask. </value>
		[XmlElement(ElementName = "day_mask")]
		public EnumDayMask DayMask { get; set; }

        /// <summary> Gets or sets the recordings to keep. </summary>
        /// <value> The recordings to keep. </value>
		[XmlElement(ElementName = "recordings_to_keep")]
		public int RecordingsToKeep { get; set; }

		#endregion

	}
}
