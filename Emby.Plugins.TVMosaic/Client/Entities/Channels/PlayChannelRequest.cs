using System;
using System.ComponentModel;
using System.Net;
using System.Xml.Serialization;
using TSoft.TVServer.Constants;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> A play channel request. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
	[XmlRoot("stream")]
    public class PlayChannelRequest : IRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.PlayChannelRequest
        /// class. </summary>
        public PlayChannelRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.PlayChannelRequest
        /// class. </summary>
        /// <param name="server_address"> The server address. </param>
        /// <param name="channel_id"> Identifier for the channel. </param>
        /// <param name="client_id"> Identifier for the client. </param>
        /// <param name="stream_type"> Type of the stream. </param>
        /// <param name="transcoder"> The transcoder. </param>
        public PlayChannelRequest(string server_address, string channel_id, string client_id, EnumStreamType stream_type, Transcoder transcoder)
        {
            ServerAddress = server_address;
            ChannelDVBLinkID = channel_id;
            ClientId = client_id;
            StreamType = stream_type;
            Transcoder = transcoder;
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.PlayChannelRequest
        /// class. </summary>
        /// <param name="server_address"> The server address. </param>
        /// <param name="physical_channel_id"> Identifier for the physical channel. </param>
        /// <param name="source_name"> Name of the source. </param>
        /// <param name="client_id"> Identifier for the client. </param>
        /// <param name="stream_type"> Type of the stream. </param>
        /// <param name="transcoder"> The transcoder. </param>
        public PlayChannelRequest(string server_address, string physical_channel_id, string source_name, string client_id, EnumStreamType stream_type, Transcoder transcoder)
        {
            ServerAddress = server_address;
            PhysicalChannelId = physical_channel_id;
            SourceName = source_name;
            ClientId = client_id;
            StreamType = stream_type;
            Transcoder = transcoder;
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.PlayChannelRequest
        /// class. </summary>
        /// <param name="server_address"> The server address. </param>
        /// <param name="channel_id"> Identifier for the channel. </param>
        /// <param name="client_id"> Identifier for the client. </param>
        /// <param name="stream_type"> Type of the stream. </param>
        public PlayChannelRequest(string server_address, string channel_id, string client_id, EnumStreamType stream_type)
            : this(server_address, channel_id, client_id, stream_type, null)
        {
        }

        public PlayChannelRequest(string server_address, string physical_channel_id, string source_name, string client_id, EnumStreamType stream_type)
                    : this(server_address, physical_channel_id, source_name, client_id, stream_type, null)
        {
        }

        #endregion

        #region Properties

        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "play_channel"; }
        }

        /// <summary> Gets or sets the identifier of the channel dvb link. </summary>
        /// <value> The identifier of the channel dvb link. </value>
        [XmlElement(ElementName = "channel_dvblink_id")]
        public string ChannelDVBLinkID { get; set; }

        /// <summary> Gets or sets the identifier of the client. </summary>
        /// <value> The identifier of the client. </value>
        [XmlElement(ElementName = "client_id")]
        public string ClientId { get; set; }

        /// <summary> Gets or sets the type of the stream. </summary>
        /// <value> The type of the stream. </value>
        [XmlElement(ElementName = "stream_type")]
        public EnumStreamType StreamType { get; set; }

        /// <summary> Gets or sets the server address. </summary>
        /// <value> The server address. </value>
        [XmlElement(ElementName = "server_address")]
        public string ServerAddress { get; set; }

        /// <summary> Gets or sets the client address. </summary>
        /// <value> The client address. </value>
        [XmlElement(ElementName = "client_address"), DefaultValue("")]
        public string ClientAddress { get; set; }

        /// <summary> Gets or sets the streaming port. </summary>
        /// <value> The streaming port. </value>
        [XmlElement(ElementName = "streaming_port"), DefaultValue(0)]
        public ushort StreamingPort { get; set; }

        /// <summary> Gets or sets the transcoder. </summary>
        /// <value> The transcoder. </value>
        [XmlElement(ElementName = "transcoder"), DefaultValue(null)]
        public Transcoder Transcoder { get; set; }

        /// <summary> Gets or sets the duration. </summary>
        /// <value> The duration. </value>
        [XmlElement(ElementName = "duration"), DefaultValue(0)]
        public long Duration { get; set; }

        /// <summary> Gets or sets the identifier of the physical channel. </summary>
        /// <value> The identifier of the physical channel. </value>
        [XmlElement(ElementName = "physical_channel_id"), DefaultValue("")]
        public string PhysicalChannelId { get; set; }

        /// <summary> Gets or sets the name of the source. </summary>
        /// <value> The name of the source. </value>
        [XmlElement(ElementName = "source_id"), DefaultValue("")]
        public string SourceName { get; set; }

        #endregion

    }
}
