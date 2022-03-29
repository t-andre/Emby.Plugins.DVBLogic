using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;
using TSoft.TVServer.Constants;

namespace TSoft.TVServer.Entities
{
	/// <summary> A pattern schedule. </summary>
	[XmlRoot("by_pattern")]
	[XmlType(AnonymousType = true)]
	public class PatternSchedule
	{
		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the TSoft.TVServer.Entities.PatternSchedule class. </summary>
		/// <param name="channel_id"> Identifier for the channel. </param>
		/// <param name="title"> The title. </param>
		/// <param name="genreMask"> The genre mask. </param>
		/// <param name="keyPhrase"> The key phrase. </param>
		public PatternSchedule(string channel_id, string title, EnumGenreMask genreMask, string keyPhrase)
		{
			ChannelID = channel_id;
			Title = title;
			KeyPhrase = keyPhrase;
			GenreMask = genreMask;
		}

		/// <summary>
		/// Initializes a new instance of the TSoft.TVServer.Entities.PatternSchedule class. </summary>
		public PatternSchedule()
		{
		}

		#endregion

		#region [Public Properties]
		/// <summary> Gets or sets the identifier of the channel. </summary>
		/// <value> The identifier of the channel. </value>
		[XmlElement(ElementName = "channel_id")]
		public string ChannelID { get; set; }

		/// <summary> Gets or sets the key phrase. </summary>
		/// <value> The key phrase. </value>
		[XmlElement(ElementName = "key_phrase"), DefaultValue("")]
		public string KeyPhrase { get; set; }

		/// <summary> Gets or sets the genre mask. </summary>
		/// <value> The genre mask. </value>
		[XmlElement(ElementName = "genre_mask")]
		public EnumGenreMask? GenreMask { get; set; }

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

		/// <summary> Gets or sets the recordings to keep. </summary>
		/// <value> The recordings to keep. </value>
		[XmlElement(ElementName = "recordings_to_keep")]
		public int RecordingsToKeep { get; set; }

		/// <summary> Gets or sets the title. </summary>
		/// <value> The title. </value>
		[XmlElement(ElementName = "title"), DefaultValue("")]
		public string Title { get; set; }

		#endregion

	}
}
