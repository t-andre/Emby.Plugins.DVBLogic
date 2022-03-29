using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Interfaces;
using TSoft.TVServer.Constants;

namespace TSoft.TVServer.Entities
{
	[XmlRoot("object_requester")]

    /// <summary> A server objects request. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
    public class ObjectsRequest : IRequest
	{
       #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ServerObjectsRequest
        /// class. </summary>
        /// <param name="server_address"> The server address. </param>
        /// <param name="object_id"> Identifier for the object. </param>
        public ObjectsRequest(string server_address, string object_id)
		{
			ObjectId = "";
			ServerAddress = server_address;
			ObjectId = object_id;
            //ObjectType = EnumObjectType.OBJECT_UNKNOWN;
            //ItemType = EnumItemType.ITEM_UNKNOWN;
            ChildrenRequest = true;
			RequestedCount = -1;
		}

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ServerObjectsRequest
        /// class. </summary>
        /// <param name="object_id"> Identifier for the object. </param>
		public ObjectsRequest(string object_id)
		{
			ObjectId = "";
			ObjectId = object_id;
			ObjectType = EnumObjectType.OBJECT_UNKNOWN;
			ItemType = EnumItemType.ITEM_UNKNOWN;
			ChildrenRequest = true;
			RequestedCount = -1;
		}

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ServerObjectsRequest
        /// class. </summary>
		public ObjectsRequest()
		{
			ObjectId = "";
			ObjectType = EnumObjectType.OBJECT_UNKNOWN;
			ItemType = EnumItemType.ITEM_UNKNOWN;
			ChildrenRequest = true;
			RequestedCount = -1;
		}

        #endregion

        #region [Constants]
        /// <summary> The container built in. </summary>
        public const string ContainerBuiltIn = "8F94B459-EFC0-4D91-9B29-EC3D72E92677";

        /// <summary> Name of the container recordings by. </summary>
        public const string ContainerRecordingsByName = "E44367A7-6293-4492-8C07-0E551195B99F";

        /// <summary> The container recordings by date. </summary>
        public const string ContainerRecordingsByDate = "F6F08949-2A07-4074-9E9D-423D877270BB";

        /// <summary> The container by genres. </summary>
        public const string ContainerByGenres = "CE482DD8-BC5E-47c3-9072-2554B968F27C";

        /// <summary> The container by series. </summary>
        public const string ContainerBySeries = "0E03FEB8-BD8F-46e7-B3EF-34F6890FB458";

        #endregion

        #region [Public Properties]
        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "get_object"; }
        }

        /// <summary> Gets or sets the identifier of the object. </summary>
        /// <value> The identifier of the object. </value>
		[XmlElement(ElementName = "object_id")]
		public string ObjectId { get; set; }

        /// <summary> Gets or sets the type of the object. </summary>
        /// <value> The type of the object. </value>
		[XmlElement(ElementName = "object_type")]
		public EnumObjectType ObjectType { get; set; }

        /// <summary> Gets or sets the type of the item. </summary>
        /// <value> The type of the item. </value>
		[XmlElement(ElementName = "item_type")]
		public EnumItemType ItemType { get; set; }

        /// <summary> Gets or sets the start position. </summary>
        /// <value> The start position. </value>
		[XmlElement(ElementName = "start_position"), DefaultValue(0)]
		public int StartPosition { get; set; }

        /// <summary> Gets or sets the number of requested. </summary>
        /// <value> The number of requested. </value>
		[XmlElement(ElementName = "requested_count")]
		public int RequestedCount { get; set; }

        /// <summary> Gets or sets a value indicating whether the children request. </summary>
        /// <value> true if children request, false if not. </value>
		[XmlElement(ElementName = "children_request")]
		public bool ChildrenRequest { get; set; }

        /// <summary> Gets or sets the server address. </summary>
        /// <value> The server address. </value>
		[XmlElement(ElementName = "server_address")]
		public string ServerAddress { get; set; }

		#endregion

	}
}
