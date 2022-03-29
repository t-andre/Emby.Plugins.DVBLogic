using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> A streaming capabilities request. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
	[XmlRoot("get_streaming_capabilities")]
    public class StreamingCapabilitiesRequest : IRequest
    {
        #region [Public properties]
        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "get_streaming_capabilities"; }
        }
        #endregion
    }
}
