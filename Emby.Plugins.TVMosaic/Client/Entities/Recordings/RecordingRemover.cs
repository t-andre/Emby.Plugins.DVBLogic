using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> A recording remover. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
	[XmlRoot("remove_recording")]
    public class RecordingRemover : IRequest
	{
		#region [Constructors]
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.RecordingRemover class. </summary>
		public RecordingRemover()
		{
		}

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.RecordingRemover class. </summary>
        /// <param name="recording_id"> Identifier for the recording. </param>
		public RecordingRemover(string recording_id)
		{
			RecordingID = recording_id;
		}

		#endregion

		#region [Public Properties]
        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "remove_recording"; }
        }

        /// <summary> Gets or sets the identifier of the recording. </summary>
        /// <value> The identifier of the recording. </value>
		[XmlElement(ElementName = "recording_id")]
		public string RecordingID { get; set; }

		#endregion

	}
}
