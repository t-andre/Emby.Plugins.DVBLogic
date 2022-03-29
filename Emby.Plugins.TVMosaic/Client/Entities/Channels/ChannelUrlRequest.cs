using System;
using System.Net;
using System.Collections.Generic;
using System.Xml.Serialization;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> A channel URL request. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
    [XmlRoot("stream_info")]
    public class ChannelUrlRequest : IRequest
    {
        #region [Constructors]
        /// <summary>
        /// initializes a new instance of the TSoft.TVServer.Entities.ChannelUrlRequest class. </summary>
        public ChannelUrlRequest()
        {
        }

        /// <summary>
        /// initializes a new instance of the TSoft.TVServer.Entities.ChannelUrlRequest class. </summary>
        /// <param name="clientId"> The identifier of the client. </param>
        /// <param name="channelid"> The channelid. </param>
        public ChannelUrlRequest(string clientId, long channelid)
        {
            ClientId = clientId;
            this.Channels = new List<long>
            {
                channelid
            };
        }

        /// <summary>
        /// initializes a new instance of the TSoft.TVServer.Entities.ChannelUrlRequest class. </summary>
        /// <param name="clientId"> The identifier of the client. </param>
        /// <param name="channelsid"> The channelsid. </param>
        public ChannelUrlRequest(string clientId, List<long> channelsid)
        {
            ClientId = clientId;
            this.Channels = channelsid;
        }

        #endregion

        #region [Public properties]
        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "get_channel_url"; }
        }

        /// <summary> Gets or sets the identifier of the client. </summary>
        /// <value> The identifier of the client. </value>
        [XmlElement(ElementName = "client_id")]
        public string ClientId { get; set; }

        /// <summary> Gets or sets the channels. </summary>
        /// <value> The channels. </value>
        [XmlArray("channels_dvblink_ids"), XmlArrayItem("channel_dvblink_id")]
        public List<long> Channels { get; set; } = new List<long>();
        #endregion
    }
}
