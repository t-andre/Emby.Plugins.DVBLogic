// <copyright file="ObjectItemVideo.cs" >
// Copyright (c) 2018 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares</author>
// <date>22.08.2018</date>
// <summary>Implements the object item video class</summary>
using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;

namespace TSoft.TVServer.Entities
{
	/// <summary> An object item video. </summary>
	[XmlRoot("video")]
	public class ObjectItemVideo
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
		/// <value> True if we can be deleted, false if not. </value>
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

		/// <summary> Gets or sets information describing the video. </summary>
		/// <value> Information describing the video. </value>
		[XmlElement(ElementName = "video_info"), DefaultValue(null)]
		public ObjectItemVideoInfo VideoInfo { get; set; }

		#endregion

	}
}
