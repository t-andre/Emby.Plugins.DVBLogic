using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSoft.TVServer.Constants;
using TSoft.TVServer.Entities;

namespace TSoft.TVServer.Helpers
{
	/// <summary> A transcoder helper. </summary>
	public static class TranscoderHelper
	{
		#region Public Methods
		/// <summary> Ge stream source. </summary>
		/// <param name="profile"> The profile. </param>
		/// <param name="directStreamRequest"> (Optional) true to direct stream request. </param>
		/// <param name="timeshift"> (Optional) true to timeshift. </param>
		/// <param name="transcode"> (Optional) true to transcode. </param>
		/// <returns> A StreamSourceInfo. </returns>
		public static StreamSourceInfo GeStreamSource(EnumProfiles profile, bool directStreamRequest = false, bool timeshift = false, bool transcode = false)
		{
			EnumStreamType streamType = EnumStreamType.STREAM_RAW_HTTP;
			Transcoder transcoder = null;
			bool isDefault = false;
			switch (profile)
			{
				case EnumProfiles.NATIVE:
					transcoder = GetTranscoder(container: "ts", videoCodec: "", audioCodec: null, isInterlaced: true);
					streamType = timeshift ? EnumStreamType.STREAM_RAW_HTTP_TIMESHIFT : EnumStreamType.STREAM_RAW_HTTP;
					transcode = false;
					isDefault = true;
					break;
				case EnumProfiles.TSPASSTHROUGH:
					transcoder = GetTranscoder(container: "ts", videoCodec: "", audioCodec: "", isInterlaced: false);
					streamType = timeshift ? EnumStreamType.STREAM_H264TS_HTTP_TIMESHIFT : EnumStreamType.STREAM_H264TS;
					transcode = false;
					break;
				case EnumProfiles.TSMOBILE:
					transcoder = GetTranscoder720("ts", "", "", 2000000, false, "", "", 2000000, 128000);
					streamType = timeshift ? EnumStreamType.STREAM_H264TS_HTTP_TIMESHIFT : EnumStreamType.STREAM_H264TS;
					break;
				case EnumProfiles.TS1080:
					transcoder = GetTranscoder1080("ts", "", "", 8000000, false, "", "", 8000000, 128000);
					streamType = timeshift ? EnumStreamType.STREAM_H264TS_HTTP_TIMESHIFT : EnumStreamType.STREAM_H264TS;
					break;
				case EnumProfiles.TS720:
					transcoder = GetTranscoder720("ts", "", "", 5000000, false, "", "", 5000000, 128000);
					streamType = timeshift ? EnumStreamType.STREAM_H264TS_HTTP_TIMESHIFT : EnumStreamType.STREAM_H264TS;
					break;
				case EnumProfiles.TS576:
					transcoder = GetTranscoder576("ts", "", "", 2500000, false, "", "", 2500000, 128000);
					streamType = timeshift ? EnumStreamType.STREAM_H264TS_HTTP_TIMESHIFT : EnumStreamType.STREAM_H264TS;
					break;
				case EnumProfiles.TS480:
					transcoder = GetTranscoder480("ts", "", "", 2000000, false, "", "", 2000000, 128000);
					streamType = timeshift ? EnumStreamType.STREAM_H264TS_HTTP_TIMESHIFT : EnumStreamType.STREAM_H264TS;
					break;
				case EnumProfiles.TS360:
					transcoder = GetTranscoder360("ts", "", "", 1500000, false, "", "", 1500000, 128000);
					streamType = timeshift ? EnumStreamType.STREAM_H264TS_HTTP_TIMESHIFT : EnumStreamType.STREAM_H264TS;
					break;
				case EnumProfiles.TS240:
					transcoder = GetTranscoder240("ts", "", "", 1000000, false, "", "", 1500000, 1000000);
					streamType = timeshift ? EnumStreamType.STREAM_H264TS_HTTP_TIMESHIFT : EnumStreamType.STREAM_H264TS;
					break;
			}
			//streamType = EnumStreamType.STREAM_HLS;
			StreamSourceInfo streamSourceInfo = new StreamSourceInfo
			{
				Id = string.Format("{0:N}", Guid.NewGuid()),
				Name = profile.ToString("F"),
				StreamType = streamType,
				Timeshift = timeshift,
				Profile = profile,
				Transcode = transcode,
				DirectStreamRequest = directStreamRequest,
				Transcoder = transcoder,
				IsDefault = isDefault
			};

			//if (!transcode)
			//{
			//    streamSourceInfo.Transcoder.AudioCodec = "aac";
			//    streamSourceInfo.Transcoder.VideoCodec = "ts";
			//    streamSourceInfo.StreamType = timeshift ? EnumStreamType.STREAM_RAW_HTTP_TIMESHIFT : EnumStreamType.STREAM_RAW_HTTP;
			//}
			return streamSourceInfo;
		}

		/// <summary> Ge stream source. </summary>
		/// <param name="profile"> The profile. </param>
		/// <param name="directStreamRequest"> (Optional) true to direct stream request. </param>
		/// <param name="timeshift"> (Optional) true to timeshift. </param>
		/// <param name="transcode"> (Optional) true to transcode. </param>
		/// <returns> A StreamSourceInfo. </returns>
		public static StreamSourceInfo GeStreamSource(string profile, bool directStreamRequest = false, bool timeshift = false, bool transcode = false)
		{
			EnumProfiles p = profile.ToEnum<EnumProfiles>(EnumProfiles.NATIVE);
			return GeStreamSource(p, directStreamRequest, timeshift, transcode);
		}

		/// <summary> Ge stream sources. </summary>
		/// <param name="directStreamRequest"> (Optional) true to direct stream request. </param>
		/// <param name="timeshift"> (Optional) true to timeshift. </param>
		/// <returns> A List&lt;StreamSourceInfo&gt; </returns>
		public static List<StreamSourceInfo> GeStreamSources(bool directStreamRequest = false, bool timeshift = false)
		{
            List<StreamSourceInfo> lst = new List<StreamSourceInfo>
            {
                GeStreamSource(EnumProfiles.NATIVE, directStreamRequest, timeshift),

                GeStreamSource(EnumProfiles.TSMOBILE, directStreamRequest, timeshift),
                GeStreamSource(EnumProfiles.TSPASSTHROUGH, directStreamRequest, timeshift),
                GeStreamSource(EnumProfiles.TS1080, directStreamRequest, timeshift),
                GeStreamSource(EnumProfiles.TS720, directStreamRequest, timeshift),
                GeStreamSource(EnumProfiles.TS576, directStreamRequest, timeshift),
                GeStreamSource(EnumProfiles.TS480, directStreamRequest, timeshift),
                GeStreamSource(EnumProfiles.TS360, directStreamRequest, timeshift),
                GeStreamSource(EnumProfiles.TS240, directStreamRequest, timeshift)
            };

            return lst;
		}

		/// <summary> Gets transcoder 1080. </summary>
		/// <param name="container"> (Optional) The container. </param>
		/// <param name="videoCodec"> (Optional) The video codec. </param>
		/// <param name="audioCodec"> (Optional) The audio codec. </param>
		/// <param name="bitrate"> (Optional) The bitrate. </param>
		/// <param name="isInterlaced"> (Optional) true if this
		/// MediaBrowser.Plugins.DVBLink.Models.Transcoder is interlaced, false if not. </param>
		/// <param name="language"> (Optional) The language. </param>
		/// <param name="audioTrack"> (Optional) The audio track. </param>
		/// <param name="videoBitrate"> (Optional) The video bitrate. </param>
		/// <param name="audioBitrate"> (Optional) The audio bitrate. </param>
		/// <returns> The transcoder 1080. </returns>
		public static Transcoder GetTranscoder1080(string container = "", string videoCodec = "", string audioCodec = "", int? bitrate = null, bool isInterlaced = false, string language = "", string audioTrack = "", int? videoBitrate = null, int? audioBitrate = null)
		{
			return GetTranscoder(1920, 1080, container, videoCodec, audioCodec, bitrate, isInterlaced, language, audioTrack, videoBitrate, audioBitrate);
		}

		/// <summary> Gets a transcoder. </summary>
		/// <param name="width"> (Optional) The width. </param>
		/// <param name="height"> (Optional) The height. </param>
		/// <param name="container"> (Optional) The container. </param>
		/// <param name="videoCodec"> (Optional) The video codec. </param>
		/// <param name="audioCodec"> (Optional) The audio codec. </param>
		/// <param name="bitrate"> (Optional) The bitrate. </param>
		/// <param name="isInterlaced"> (Optional) true if this
		/// MediaBrowser.Plugins.DVBLink.Models.Transcoder is interlaced, false if not. </param>
		/// <param name="language"> (Optional) The language. </param>
		/// <param name="audioTrack"> (Optional) The audio track. </param>
		/// <param name="videoBitrate"> (Optional) The video bitrate. </param>
		/// <param name="audioBitrate"> (Optional) The audio bitrate. </param>
		/// <returns> The transcoder. </returns>
		public static Transcoder GetTranscoder(int? width = null, int? height = null, string container = "", string videoCodec = "", string audioCodec = "", int? bitrate = null, bool isInterlaced = false, string language = "", string audioTrack = "", int? videoBitrate = null, int? audioBitrate = null)
		{
			return new Transcoder
			{
				Height = height,
				Width = width,
				Bitrate = bitrate,
				AudioTrack = audioTrack,
				VideoBitrate = videoBitrate,
				AudioBitrate = audioBitrate,
				IsInterlaced = isInterlaced,
				VideoCodec = videoCodec,
				AudioCodec = audioCodec,
				Container = container,
				Language = language,
				Profile = "baseline",
				Level = 41
			};
		}

		/// <summary> Gets transcoder 240. </summary>
		/// <param name="container"> (Optional) The container. </param>
		/// <param name="videoCodec"> (Optional) The video codec. </param>
		/// <param name="audioCodec"> (Optional) The audio codec. </param>
		/// <param name="bitrate"> (Optional) The bitrate. </param>
		/// <param name="isInterlaced"> (Optional) true if this
		/// MediaBrowser.Plugins.DVBLink.Models.Transcoder is interlaced, false if not. </param>
		/// <param name="language"> (Optional) The language. </param>
		/// <param name="audioTrack"> (Optional) The audio track. </param>
		/// <param name="videoBitrate"> (Optional) The video bitrate. </param>
		/// <param name="audioBitrate"> (Optional) The audio bitrate. </param>
		/// <returns> The transcoder 240. </returns>
		public static Transcoder GetTranscoder240(string container = "", string videoCodec = "", string audioCodec = "", int? bitrate = null, bool isInterlaced = false, string language = "", string audioTrack = "", int? videoBitrate = null, int? audioBitrate = null)
		{
			return GetTranscoder(432, 240, container, videoCodec, audioCodec, bitrate, isInterlaced, language, audioTrack, videoBitrate, audioBitrate);
		}

		/// <summary> Gets transcoder 360. </summary>
		/// <param name="container"> (Optional) The container. </param>
		/// <param name="videoCodec"> (Optional) The video codec. </param>
		/// <param name="audioCodec"> (Optional) The audio codec. </param>
		/// <param name="bitrate"> (Optional) The bitrate. </param>
		/// <param name="isInterlaced"> (Optional) true if this
		/// MediaBrowser.Plugins.DVBLink.Models.Transcoder is interlaced, false if not. </param>
		/// <param name="language"> (Optional) The language. </param>
		/// <param name="audioTrack"> (Optional) The audio track. </param>
		/// <param name="videoBitrate"> (Optional) The video bitrate. </param>
		/// <param name="audioBitrate"> (Optional) The audio bitrate. </param>
		/// <returns> The transcoder 360. </returns>
		public static Transcoder GetTranscoder360(string container = "", string videoCodec = "", string audioCodec = "", int? bitrate = null, bool isInterlaced = false, string language = "", string audioTrack = "", int? videoBitrate = null, int? audioBitrate = null)
		{
			return GetTranscoder(640, 360, container, videoCodec, audioCodec, bitrate, isInterlaced, language, audioTrack, videoBitrate, audioBitrate);
		}

		/// <summary> Gets transcoder 480. </summary>
		/// <param name="container"> (Optional) The container. </param>
		/// <param name="videoCodec"> (Optional) The video codec. </param>
		/// <param name="audioCodec"> (Optional) The audio codec. </param>
		/// <param name="bitrate"> (Optional) The bitrate. </param>
		/// <param name="isInterlaced"> (Optional) true if this
		/// MediaBrowser.Plugins.DVBLink.Models.Transcoder is interlaced, false if not. </param>
		/// <param name="language"> (Optional) The language. </param>
		/// <param name="audioTrack"> (Optional) The audio track. </param>
		/// <param name="videoBitrate"> (Optional) The video bitrate. </param>
		/// <param name="audioBitrate"> (Optional) The audio bitrate. </param>
		/// <returns> The transcoder 480. </returns>
		public static Transcoder GetTranscoder480(string container = "", string videoCodec = "", string audioCodec = "", int? bitrate = null, bool isInterlaced = false, string language = "", string audioTrack = "", int? videoBitrate = null, int? audioBitrate = null)
		{
			return GetTranscoder(848, 480, container, videoCodec, audioCodec, bitrate, isInterlaced, language, audioTrack, videoBitrate, audioBitrate);
		}

		/// <summary> Gets transcoder 576. </summary>
		/// <param name="container"> (Optional) The container. </param>
		/// <param name="videoCodec"> (Optional) The video codec. </param>
		/// <param name="audioCodec"> (Optional) The audio codec. </param>
		/// <param name="bitrate"> (Optional) The bitrate. </param>
		/// <param name="isInterlaced"> (Optional) true if this
		/// MediaBrowser.Plugins.DVBLink.Models.Transcoder is interlaced, false if not. </param>
		/// <param name="language"> (Optional) The language. </param>
		/// <param name="audioTrack"> (Optional) The audio track. </param>
		/// <param name="videoBitrate"> (Optional) The video bitrate. </param>
		/// <param name="audioBitrate"> (Optional) The audio bitrate. </param>
		/// <returns> The transcoder 576. </returns>
		public static Transcoder GetTranscoder576(string container = "", string videoCodec = "", string audioCodec = "", int? bitrate = null, bool isInterlaced = false, string language = "", string audioTrack = "", int? videoBitrate = null, int? audioBitrate = null)
		{
			return GetTranscoder(720, 576, container, videoCodec, audioCodec, bitrate, isInterlaced, language, audioTrack, videoBitrate, audioBitrate);
		}

		/// <summary> Gets transcoder 720. </summary>
		/// <param name="container"> (Optional) The container. </param>
		/// <param name="videoCodec"> (Optional) The video codec. </param>
		/// <param name="audioCodec"> (Optional) The audio codec. </param>
		/// <param name="bitrate"> (Optional) The bitrate. </param>
		/// <param name="isInterlaced"> (Optional) true if this
		/// MediaBrowser.Plugins.DVBLink.Models.Transcoder is interlaced, false if not. </param>
		/// <param name="language"> (Optional) The language. </param>
		/// <param name="audioTrack"> (Optional) The audio track. </param>
		/// <param name="videoBitrate"> (Optional) The video bitrate. </param>
		/// <param name="audioBitrate"> (Optional) The audio bitrate. </param>
		/// <returns> The transcoder 720. </returns>
		public static Transcoder GetTranscoder720(string container = "", string videoCodec = "", string audioCodec = "", int? bitrate = null, bool isInterlaced = false, string language = "", string audioTrack = "", int? videoBitrate = null, int? audioBitrate = null)
		{
			return GetTranscoder(1280, 720, container, videoCodec, audioCodec, bitrate, isInterlaced, language, audioTrack, videoBitrate, audioBitrate);
		}

		#endregion

	}
}
