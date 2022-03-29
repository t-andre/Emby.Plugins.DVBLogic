using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> A recording stop request. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
	[XmlRoot("recordings")]
    public class RecordingStopRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.RecordingStopRequest
        /// class. </summary>
        /// <param name="objectId"> The identifier of the object. </param>
        public RecordingStopRequest(string objectId)
        {
            ObjectId = objectId;
        }

        #endregion

        #region Properties

        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "stop_recording"; }
        }

        /// <summary> Gets or sets the identifier of the object. </summary>
        /// <value> The identifier of the object. </value>
        [XmlElement(ElementName = "object_id")]
        public string ObjectId { get; set; }

        #endregion

    }
}
