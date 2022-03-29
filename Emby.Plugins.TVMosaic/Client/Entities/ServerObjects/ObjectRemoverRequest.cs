using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> An object remover request. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
	[XmlRoot("object_remover")]
    public class ObjectRemoverRequest : IRequest
	{
        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ObjectRemoverRequest
        /// class. </summary>
        /// <param name="object_id"> Identifier for the object. </param>
        public ObjectRemoverRequest(string object_id)
		{
			ObjectId = object_id;
		}

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ObjectRemoverRequest
        /// class. </summary>
		public ObjectRemoverRequest()
		{
		}
		#endregion

		#region [Public Properties]
        public string HttpCommand
        {
            get { return "remove_object"; }
        }

        /// <summary> Gets or sets the identifier of the object. </summary>
        /// <value> The identifier of the object. </value>
		[XmlElement(ElementName = "object_id")]
		public string ObjectId { get; set; }

		#endregion
	}
}
