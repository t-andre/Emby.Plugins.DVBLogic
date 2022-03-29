using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Constants;

namespace TSoft.TVServer.Entities
{
    /// <summary> An object item recorded tv. </summary>
	[XmlRoot("recorded_tv")]
	public class ObjectItemRecordedTV
	{
		#region [Public Properties]
        /// <summary> Gets or sets the identifier of the object. </summary>
        /// <value> The identifier of the object. </value>
        [XmlElement(ElementName = "object_id")]
		public string ObjectId { get; set; }

        /// <summary> Gets or sets the identifier of the parent. </summary>
        /// <value> The identifier of the parent. </value>
		[XmlElement(ElementName = "parent_id")]
		public string ParentId { get; set; }

        /// <summary> Gets or sets URL of the document. </summary>
        /// <value> The URL. </value>
		[XmlElement(ElementName = "url")]
		public string Url { get; set; }

        /// <summary> Gets or sets the thumbnail. </summary>
        /// <value> The thumbnail. </value>
		[XmlElement(ElementName = "thumbnail")]
		public string Thumbnail { get; set; }

        /// <summary> Gets or sets a value indicating whether we can be deleted. </summary>
        /// <value> true if we can be deleted, false if not. </value>
		[XmlElement(ElementName = "can_be_deleted")]
		public bool CanBeDeleted { get; set; }

        /// <summary> Gets or sets the size. </summary>
        /// <value> The size. </value>
        [XmlElement(ElementName = "size")]
		public long Size { get; set; }

        /// <summary> Gets or sets the creation time. </summary>
        /// <value> The creation time. </value>
		[XmlElement(ElementName = "creation_time")]
		public long CreationTime { get; set; }

        /// <summary> Gets or sets the name of the channel. </summary>
        /// <value> The name of the channel. </value>
		[XmlElement(ElementName = "channel_name"), DefaultValue("")]
		public string ChannelName { get; set; }

        /// <summary> Gets or sets the channel number. </summary>
        /// <value> The channel number. </value>
		[XmlElement(ElementName = "channel_number"), DefaultValue(0)]
		public int ChannelNumber { get; set; }

        /// <summary> Gets or sets the channel subnumber. </summary>
        /// <value> The channel subnumber. </value>
		[XmlElement(ElementName = "channel_subnumber"), DefaultValue(0)]
		public int ChannelSubnumber { get; set; }

        /// <summary> Gets or sets the identifier of the channel. </summary>
        /// <value> The identifier of the channel. </value>
        [XmlElement(ElementName = "channel_id"), DefaultValue("")]
		public string ChannelID { get; set; }

        /// <summary> Gets or sets the identifier of the schedule. </summary>
        /// <value> The identifier of the schedule. </value>
		[XmlElement(ElementName = "schedule_id"), DefaultValue("")]
		public string ScheduleID { get; set; }

        /// <summary> Gets or sets the name of the schedule. </summary>
        /// <value> The name of the schedule. </value>
		[XmlElement(ElementName = "schedule_name"), DefaultValue("")]
		public string ScheduleName { get; set; }

        /// <summary> Gets or sets a value indicating whether the schedule series. </summary>
        /// <value> true if schedule series, false if not. </value>
        [XmlElement(ElementName = "schedule_series"), DefaultValue(false)]
        public bool ScheduleSeries { get; set; }

        /// <summary> Gets or sets the state. </summary>
        /// <value> The state. </value>
		[XmlElement(ElementName = "state")]
		public EnumState State { get; set; }

        /// <summary> Gets or sets information describing the video. </summary>
        /// <value> Information describing the video. </value>
		[XmlElement(ElementName = "video_info"), DefaultValue(null)]
		public ObjectItemVideoInfo VideoInfo { get; set; }

		#endregion

	}
}
