using System;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;

namespace TSoft.TVServer.Entities
{
    /// <summary> A transcoder. </summary>
	[XmlRoot("transcoder")]
    public class Transcoder
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.Transcoder class. </summary>
        /// <param name="height"> The height. </param>
        /// <param name="width"> The width. </param>
        /// <param name="bitrate"> The bitrate. </param>
        /// <param name="audio_track"> The audio track. </param>
        public Transcoder(int height, int width, int bitrate, string audio_track)
        {
            Height = height;
            Width = width;
            Bitrate = bitrate;
            AudioTrack = audio_track;
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.Transcoder class. </summary>
        public Transcoder()
        {
        }

        #endregion

        #region Properties

        /// <summary> Gets or sets the audio bitrate. </summary>
        /// <value> The audio bitrate. </value>
        [XmlIgnore]
        public int? AudioBitrate { get; set; }

        /// <summary> Gets or sets the audio codec. </summary>
        /// <value> The audio codec. </value>
        [XmlIgnore]
        public string AudioCodec { get; set; }

        /// <summary> Gets or sets the audio track. </summary>
        /// <value> The audio track. </value>
        [XmlElement(ElementName = "audio_track"), DefaultValue("")]
        public string AudioTrack { get; set; }

        /// <summary> Gets or sets the bitrate. </summary>
        /// <value> The bitrate. </value>
        [XmlElement(ElementName = "bitrate"), DefaultValue(0)]
        public int? Bitrate { get; set; }

        /// <summary> Gets or sets the container. </summary>
        /// <value> The container. </value>
        [XmlIgnore]
        public string Container { get; set; }

        /// <summary> Gets or sets the height. </summary>
        /// <value> The height. </value>
        [XmlElement(ElementName = "height"), DefaultValue(0)]
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this TSoft.TVServer.Entities.Transcoder is
        /// interlaced. </summary>
        /// <value>
        /// true if this TSoft.TVServer.Entities.Transcoder is interlaced, false if not. </value>
        [XmlIgnore]
        public bool IsInterlaced { get; set; }

        /// <summary> Gets or sets the language. </summary>
        /// <value> The language. </value>
        [XmlIgnore]
        public string Language { get; set; }

        /// <summary> Gets or sets the level. </summary>
        /// <value> The level. </value>
        [XmlIgnore]
        public int Level { get; set; }

        /// <summary> Gets or sets the profile. </summary>
        /// <value> The profile. </value>
        [XmlIgnore]
        public string Profile { get; set; }

        /// <summary> Gets or sets the video bitrate. </summary>
        /// <value> The video bitrate. </value>
        [XmlIgnore]
        public int? VideoBitrate { get; set; }

        /// <summary> Gets or sets the video codec. </summary>
        /// <value> The video codec. </value>
        [XmlIgnore]
        public string VideoCodec { get; set; }

        /// <summary> Gets or sets the width. </summary>
        /// <value> The width. </value>
        [XmlElement(ElementName = "width"), DefaultValue(0)]
        public int? Width { get; set; }

        #endregion

    }
}
