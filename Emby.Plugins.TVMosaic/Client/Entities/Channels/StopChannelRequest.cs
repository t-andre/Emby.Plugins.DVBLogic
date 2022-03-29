using System;
using System.ComponentModel;
using System.Net;
using System.Xml.Serialization;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> A stop channel request. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
	[XmlRoot("stop_stream")]
    public class StopChannelRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.StopChannelRequest
        /// class. </summary>
        public StopChannelRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.StopChannelRequest
        /// class. </summary>
        /// <param name="channel_handle"> Handle of the channel. </param>
        public StopChannelRequest(long channel_handle)
        {
            ChannelHandle = channel_handle;
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.StopChannelRequest
        /// class. </summary>
        /// <param name="client_id"> Identifier for the client. </param>
        public StopChannelRequest(string client_id)
        {
            ClientId = client_id;
        }

        #endregion

        #region Properties

        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "stop_channel"; }
        }

        /// <summary> Gets or sets the handle of the channel. </summary>
        /// <value> The channel handle. </value>
        [XmlElement(ElementName = "channel_handle"), DefaultValue(0)]
        public long ChannelHandle { get; set; }

        /// <summary> Gets or sets the identifier of the client. </summary>
        /// <value> The identifier of the client. </value>
        [XmlElement(ElementName = "client_id"), DefaultValue("")]
        public string ClientId { get; set; }

        #endregion

    }
}
