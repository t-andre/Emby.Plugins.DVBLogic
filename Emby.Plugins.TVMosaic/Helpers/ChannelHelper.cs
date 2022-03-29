// <copyright file="ChannelHelper.cs" >
// Copyright (c) 2017 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>01.09.2017</date>
// <summary>Implements the channel helper class</summary>
using MediaBrowser.Model.LiveTv;
using MediaBrowser.Model.MediaInfo;
using TSoft.TVServer.Constants;

namespace Emby.Plugins.DVBLogic.Helpers
{
	/// <summary> A channel helper. </summary>
	public static class ChannelHelper
    {
        #region Public Methods
        /// <summary> Generates a program identifier. </summary>
        /// <param name="programID"> Identifier for the program. </param>
        /// <param name="args"> The arguments. </param>
        /// <returns> The program identifier. </returns>
        public static string GenerateProgramID(string programID, params string[] args)
        {
            return programID + (args.Length > 0 ? "|" + string.Join("|", args) : string.Empty);
        }

        /// <summary> Gets program identifier. </summary>
        /// <param name="args"> The arguments. </param>
        /// <returns> The program identifier. </returns>
        public static string GetProgramID(string args)
        {
            var value = args.Split('|');
            if (value.Length > 0)
                return value[0];
            else
                return string.Empty;
        }

        /// <summary> Gets channel type. </summary>
        /// <param name="channelType"> Type of the channel. </param>
        /// <returns> The channel type. </returns>
        public static ChannelType GetChannelType(EnumChannelType channelType)
        {
            switch (channelType)
            {
                case EnumChannelType.RD_CHANNEL_TV:
                    return ChannelType.TV;
                case EnumChannelType.RD_CHANNEL_RADIO:
                    return ChannelType.Radio;
                default:
                    return ChannelType.TV;
            }
        }

        /// <summary> Gets container type. </summary>
        /// <param name="streamType"> Type of the stream. </param>
        /// <returns> The container type. </returns>
        public static string GetContainerType(EnumStreamType streamType)
        {
            switch (streamType)
            {
                case EnumStreamType.STREAM_RAW_HTTP:
                case EnumStreamType.STREAM_RAW_HTTP_TIMESHIFT:
                    return "ts";
                case EnumStreamType.STREAM_RAW_UDP:
                    return "ts";
                case EnumStreamType.STREAM_HLS:
                    return "ts";
                case EnumStreamType.STREAM_RTP:
                    return "ts";
                case EnumStreamType.STREAM_WEBM:
                case EnumStreamType.STREAM_MP4:
                    return "mp4";
                case EnumStreamType.STREAM_H264TS:
                case EnumStreamType.STREAM_H264TS_HTTP_TIMESHIFT:
                    return "ts";
                default:
                    return "ts";
            }
        }

        /// <summary> Gets media protocol type. </summary>
        /// <param name="streamType"> Type of the stream. </param>
        /// <returns> The media protocol type. </returns>
        public static MediaProtocol GetMediaProtocolType(EnumStreamType streamType)
        {
            switch (streamType)
            {
                case EnumStreamType.STREAM_RAW_HTTP:
                case EnumStreamType.STREAM_RAW_HTTP_TIMESHIFT:
                    return MediaProtocol.Http;
                case EnumStreamType.STREAM_RAW_UDP:
                    return MediaProtocol.Http;
                case EnumStreamType.STREAM_HLS:
                    return MediaProtocol.Http;
                case EnumStreamType.STREAM_RTP:
                    return MediaProtocol.Rtsp;
                case EnumStreamType.STREAM_WEBM:
                case EnumStreamType.STREAM_MP4:
                case EnumStreamType.STREAM_H264TS:
                case EnumStreamType.STREAM_H264TS_HTTP_TIMESHIFT:
                    return MediaProtocol.Http;
                default:
                    return MediaProtocol.Http;
            }
        }

        /// <summary> Gets recording status type. </summary>
        /// <param name="state"> The state. </param>
        /// <returns> The recording status type. </returns>
        public static RecordingStatus GetRecordingStatusType(EnumState state)
        {
            switch (state)
            {
                case EnumState.RTVS_IN_PROGRESS:
                    return RecordingStatus.InProgress;
                case EnumState.RTVS_COMPLETED:
                    return RecordingStatus.Completed;
                case EnumState.RTVS_FORCED_TO_COMPLETION:
                    return RecordingStatus.Completed;
                case EnumState.RTVS_ERROR:
                    return RecordingStatus.Error;
                default:
                    return RecordingStatus.Error;
            }
        }

        #endregion

    }
}
