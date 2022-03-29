using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> A recording settings request. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
    [XmlRoot("get_recording_settings")]
    public class RecordingSettingsRequest : IRequest
    {
        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.RecordingSettingsRequest
        /// class. </summary>
        public RecordingSettingsRequest()
        {
        }

        #endregion

        #region [Public Properties]
        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "get_recording_settings"; }
        }

        #endregion

    }
}
