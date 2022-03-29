using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Constants;

namespace TSoft.TVServer.Entities
{
    /// <summary> An object container. </summary>
	[XmlRoot("container")]
	public class ObjectContainer
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

        /// <summary> Gets or sets the name. </summary>
        /// <value> The name. </value>
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }

        /// <summary> Gets or sets the description. </summary>
        /// <value> The description. </value>
		[XmlElement(ElementName = "description")]
		public string Description { get; set; }

        /// <summary> Gets or sets the logo. </summary>
        /// <value> The logo. </value>
		[XmlElement(ElementName = "logo")]
		public string Logo { get; set; }

        /// <summary> Gets or sets the type of the container. </summary>
        /// <value> The type of the container. </value>
		[XmlElement(ElementName = "container_type")]
		public EnumContainerType ContainerType { get; set; }

        /// <summary> Gets or sets the type of the content. </summary>
        /// <value> The type of the content. </value>
		[XmlElement(ElementName = "content_type")]
		public EnumItemType ContentType { get; set; }

        /// <summary> Gets or sets the number of totals. </summary>
        /// <value> The total number of count. </value>
		[XmlElement(ElementName = "total_count")]
		public int TotalCount { get; set; }

        /// <summary> Gets or sets the identifier of the source. </summary>
        /// <value> The identifier of the source. </value>
		[XmlElement(ElementName = "source_id")]
		public string SourceId { get; set; }

		#endregion

	}
}
