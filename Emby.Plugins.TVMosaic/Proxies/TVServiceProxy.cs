// <copyright file="TVServiceProxy.cs" >
// Copyright (c) 2017 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>01.09.2017</date>
// <summary>Implements the TV service proxy class</summary>
using Emby.Plugins.DVBLogic.Helpers;
using MediaBrowser.Common.Extensions;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.LiveTv;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TSoft.TVServer;
using TSoft.TVServer.Cache;
using TSoft.TVServer.Constants;
using TSoft.TVServer.Entities;
using TSoft.TVServer.Helpers;

namespace Emby.Plugins.DVBLogic.Proxies
{
    /// <summary> A TV service proxy. </summary>
    public class TVServiceProxy
    {
        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the Emby.Plugins.DVBLink.Proxies.TVServiceProxy class. </summary>
        /// <param name="config"> The configuration. </param>
        /// <param name="httpClient"> The HTTP client. </param>
        /// <param name="xmlSerializer"> The XML serializer. </param>
        public TVServiceProxy(IDVBLogicPlugin plugin, EnumTVServerClientType clientType, IHttpClient httpClient, IXmlSerializer xmlSerializer)
        {
            this.DVBLogicPlugin = plugin;
            this.Client = new TVServerClient(clientType);
            PluginHttpClient phttp = new PluginHttpClient(this.DVBLogicPlugin.Logger, httpClient, xmlSerializer);
            this.Client.Initialize(this.DVBLogicPlugin.Logger, this.DVBLogicPlugin.Config.ServerConfig, phttp);
            //this.Client.Initialize(Plugin.Logger, config);
        }

        #endregion

        #region [Private fields]

        /// <summary> The channel cache. </summary>
        private readonly CacheCollection<ChannelInfo> _ChannelCache = new CacheCollection<ChannelInfo>(TimeSpan.FromMinutes(60));

        /// <summary> The recording cache. </summary>
        private readonly CacheCollection<RecordingInfo> _RecordingCache = new CacheCollection<RecordingInfo>(TimeSpan.FromMinutes(2));

        /// <summary>	all recording cache. </summary>
        private readonly CacheCollection<MyRecordingInfo> _AllRecordingCache = new CacheCollection<MyRecordingInfo>(TimeSpan.FromMinutes(2));

        /// <summary> The timers cache. </summary>
        private readonly CacheCollection<TimerInfo> _TimersCache = new CacheCollection<TimerInfo>(TimeSpan.FromMinutes(2));

        /// <summary> The series timers cache. </summary>
        private readonly CacheCollection<SeriesTimerInfo> _SeriesTimersCache = new CacheCollection<SeriesTimerInfo>(TimeSpan.FromMinutes(2));

        /// <summary> Full pathname of the logos file. </summary>
        private string _LogosPath = string.Empty;

        /// <summary> True to direct stream. </summary>
        private bool _DirectStream = true;

        /// <summary> The default profile. </summary>
        private readonly EnumProfiles _DefaultProfile = EnumProfiles.NATIVE;

        #endregion

        #region [Properties]
        /// <summary> Gets the client. </summary>
        /// <value> The client. </value>
        internal TVServerClient Client { get; }

        /// <summary> Gets the dvb logic plugin. </summary>
        /// <value> The dvb logic plugin. </value>
        protected IDVBLogicPlugin DVBLogicPlugin { get; }

        #endregion

        #region [Server]
        /// <summary> Gets status information asynchronous. </summary>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> The asynchronous result that yields the status information asynchronous. </returns>
        public async Task<LiveTvServiceStatusInfo> GetStatusInfoAsync(CancellationToken cancellationToken)
        {
            this.DVBLogicPlugin.Logger.Info("Get Server Status Info");
            var result = await this.Client.GetServerInfoAsync(cancellationToken).ConfigureAwait(false);

            var status = new LiveTvServiceStatusInfo
            {
                HasUpdateAvailable = false,
                Status = (this.Client.Status.IsAvailable && this.Client.Enable) ? MediaBrowser.Model.LiveTv.LiveTvServiceStatus.Ok : MediaBrowser.Model.LiveTv.LiveTvServiceStatus.Unavailable,
                IsVisible = this.Client.Enable
            };

            if (this.Client.Status.IsAvailable)
                status.StatusMessage = string.Empty;
            else if (!this.Client.Enable)
                status.StatusMessage = $"The plugin {this.Client.Name} is disable.";
            else
                status.StatusMessage = $"Cannot retrieve {this.Client.Name} Server info.";

            if (result != null)
            {
                status.Version = result.Version;
            }
            else
            {
                status.Version = $"This version of {this.Client.Name} does not support server version request";
            }
            return status;
        }

        #endregion

        #region [Channels]
        /// <summary> Gets channels asynchronous. </summary>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <param name="logoPath"> (Optional) Full pathname of the logo file. </param>
        /// <returns>
        /// The asynchronous result that yields an enumerator that allows foreach to be used to process
        /// the channels asynchronous in this collection. </returns>
        public async Task<IEnumerable<ChannelInfo>> GetChannelsAsync(CancellationToken cancellationToken, string logoPath = "")
        {
            if (!this.Client.Enable) return new List<ChannelInfo>();

            var channels = new List<ChannelInfo>();
            this._LogosPath = logoPath;
            DataCache<ChannelInfo> cache = this._ChannelCache.GetCache(this.Client.TunerInfo.Id);

            if (cache?.IsValid == true)
            {
                this.DVBLogicPlugin.Logger.Info("Channels from cache: {0}", cache.Data.Count);
                return cache.Data;
            }

            this.DVBLogicPlugin.Logger.Info("Get Channels");

            var result = await this.Client.GetChannelsAsync(cancellationToken).ConfigureAwait(false);
            var favorites = await this.Client.GetFavoritesAsync(cancellationToken).ConfigureAwait(false);

            if (result != null)
            {
                this.DVBLogicPlugin.Logger.Info("Channels found on server: {0}", result.Items.Count);
                List<string> favoriteChannels = new List<string>();
                if (favorites != null)
                {
                    var query = favorites.Items
                        .Where
                        (c =>
                                string.Compare(c.Id, this.DVBLogicPlugin.Config.OtherOptions.Favourite1, StringComparison.OrdinalIgnoreCase) == 0
                                || string.Compare(c.Id, this.DVBLogicPlugin.Config.OtherOptions.Favourite2, StringComparison.OrdinalIgnoreCase) == 0
                                || string.Compare(c.Id, this.DVBLogicPlugin.Config.OtherOptions.Favourite3, StringComparison.OrdinalIgnoreCase) == 0
                                || string.Compare(c.Id, this.DVBLogicPlugin.Config.OtherOptions.Favourite4, StringComparison.OrdinalIgnoreCase) == 0
                                || string.Compare(c.Id, this.DVBLogicPlugin.Config.OtherOptions.Favourite5, StringComparison.OrdinalIgnoreCase) == 0
                        )
                        .SelectMany(x => x.Channels);

                    favoriteChannels.AddRange(query.ToList());
                }

                result = this.Client.UpdateChannels(result, this._LogosPath);
                var selectedChannels = result.Items;
                if (favoriteChannels.Count > 0)
                {
                    selectedChannels = (from c in result.Items
                                        from fc in favoriteChannels
                                        where c.ChannelID == fc
                                        select c).ToList();
                }
                foreach (Channel channel in selectedChannels)
                {
                    try
                    {
                        string channelUrl = this.Client.GetDirectChannelStream(this.Client.ClientId, channel.ChannelDVBLinkID); ;
                        var chi = new ChannelInfo
                        {
                            Id = channel.ChannelDVBLinkID,
                            Name = channel.Name,
                            Number = channel.Number.ToString(),
                            ImageUrl = channel.HasChannelLogo && channel.ServerLogo ? channel.ChannelLogo : string.Empty,
                            ImagePath = channel.HasChannelLogo && !channel.ServerLogo ? channel.ChannelLogo : string.Empty,
                            HasImage = channel.HasChannelLogo,
                            ChannelType = ChannelHelper.GetChannelType(channel.Type),
                            TunerHostId = Client.TunerInfo.Id,
                            IsHD = channel.IsHD,
                            Path = channelUrl
                        };
                        channels.Add(chi);
                    }
                    catch (Exception ex)
                    {
                        this.DVBLogicPlugin.Logger.ErrorException("Add channel Error : {0}", ex, channel.Number.ToString());
                    }
                }
                this._ChannelCache.SetCache(this.Client.TunerInfo.Id, cache, channels);
                this.DVBLogicPlugin.Logger.Info("Channels saved to cache");
            }

            return channels;
        }

        #endregion

        #region [Epg]
        /// <summary> Gets programs asynchronous. </summary>
        /// <param name="channelId"> Identifier for the channel. </param>
        /// <param name="startDateUtc"> The start date UTC. </param>
        /// <param name="endDateUtc"> The end date UTC. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns>
        /// The asynchronous result that yields an enumerator that allows foreach to be used to process
        /// the programs asynchronous in this collection. </returns>
        public async Task<IEnumerable<ProgramInfo>> GetProgramsAsync(string channelId, DateTimeOffset startDateUtc, DateTimeOffset endDateUtc, CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return new List<ProgramInfo>();

            this.DVBLogicPlugin.Logger.Info(string.Format("Get Programs, retrieve all programs for ChannelId: {0}", channelId));
            var programList = new List<ProgramInfo>();
            var request = new EpgRequest(channelId, startDateUtc.GetCurrentUnixTimestampOffsetSeconds(), endDateUtc.GetCurrentUnixTimestampOffsetSeconds());
            var result = await this.Client.GetEpgAsync(request, cancellationToken).ConfigureAwait(false);
            if (result != null)
            {
                if (result.Items.FirstOrDefault()?.Programs != null)
                {
                    this.DVBLogicPlugin.Logger.Info("Programs found for channel : {0} - {1}", channelId, result.Items.Count);

                    foreach (var item in result.Items.FirstOrDefault()?.Programs.Items)
                    {
                        string programId = ChannelHelper.GenerateProgramID(item.ProgramID, channelId);
                        try
                        {
                            int? year = (int)item.Year;
                            if (year <= 0)
                                year = null;

                            int? seasonNumber = (int)item.SeasonNum;
                            if (seasonNumber <= 0)
                                seasonNumber = null;

                            int? episodeNum = (int)item.EpisodeNum;
                            if (episodeNum <= 0)
                                episodeNum = null;
                            var program = new ProgramInfo
                            {
                                Id = programId,
                                ChannelId = channelId,
                                //Id = item.ID,
                                Overview = item.ShortDesc,
                                StartDate = item.GetStarDateOffset(),
                                EndDate = item.GetEndDateOffset(),
                                HasImage = !string.IsNullOrEmpty(item.Image),
                                ImageUrl = item.Image,
                                //ThumbImageUrl = item.Image,
                                IsRepeat = item.Repeat,
                                IsPremiere = item.Premiere,
                                IsHD = item.Hdtv,
                                IsMovie = item.CatMovie,
                                IsNews = item.CatNews,
                                IsSeries = item.IsSeries,
                                IsSports = item.CatSports,
                                IsKids = item.CatKids,
                                IsEducational = item.CatEducational,
                                ProductionYear = year,
                                SeasonNumber = seasonNumber,
                                EpisodeNumber = episodeNum
                            };

                            if (!string.IsNullOrEmpty(item.Name))
                            {
                                program.Name = item.Name;
                            }

                            if (!string.IsNullOrEmpty(item.Subname))
                            {
                                program.EpisodeTitle = item.Subname;
                            };

                            if (!string.IsNullOrEmpty(item.Categories))
                            {
                                program.Genres = new List<string>(item.Categories.ToString().Split('/'));
                            }
                            programList.Add(program);
                        }
                        catch (Exception ex)
                        {
                            this.DVBLogicPlugin.Logger.ErrorException("Add program Error for channel : {0} - {1}", ex, channelId, programId);
                        }
                    }
                }
            }
            return programList;
        }

        #endregion

        #region [Recordings]

        /// <summary> Gets all recordings asynchronous. </summary>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> An asynchronous result that yields all recordings. </returns>
        public async Task<IEnumerable<MyRecordingInfo>> GetAllRecordingsAsync(CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return new List<MyRecordingInfo>();

            DataCache<MyRecordingInfo> cache = this._AllRecordingCache.GetCache(this.Client.TunerInfo.Id);

            if (cache?.IsValid == true)
            {
                this.DVBLogicPlugin.Logger.Info("Recordings from cache: {0}", cache.Data.Count);
                return cache.Data;
            }

            this.DVBLogicPlugin.Logger.Info("Get Recordings");

            var recordingInfos = new List<MyRecordingInfo>();
            var result = await this.Client.GetObjectsRecordingsByNameAsync(cancellationToken).ConfigureAwait(false);
            if (result != null)
            {
                if (result.Items?.RecordedTv.Count > 0)
                {
                    this.DVBLogicPlugin.Logger.Info("Recordings found on server: {0}", result.Items.RecordedTv.Count);
                    foreach (ObjectItemRecordedTV item in result.Items.RecordedTv)
                    {
                        try
                        {
                            var streamSourceInfo = TranscoderHelper.GeStreamSource(this._DefaultProfile, this._DirectStream, this.Client.Config.EnableTimeshift, this.Client.Config.Transcode);
                            streamSourceInfo.state = item.State;
                            streamSourceInfo.StartDate = item.VideoInfo.GetStarDateOffset();
                            streamSourceInfo.EndDate = item.VideoInfo.GetEndDateOffset();
                            MediaSourceInfo mediaSourceInfo = this.GetMediaSource(streamSourceInfo);
                            mediaSourceInfo.Size = item.Size;
                            if (item.VideoInfo != null && item.VideoInfo.Duration > 0)
                                mediaSourceInfo.RunTimeTicks = TimeSpan.FromSeconds(item.VideoInfo.Duration).Ticks;

                            mediaSourceInfo.Id = item.ObjectId;
                            mediaSourceInfo.Path = item.Url;
                            mediaSourceInfo.IsInfiniteStream = false;

                            var recordingInfo = new MyRecordingInfo
                            {
                                Id = item.ObjectId,
                                ChannelId = item.ChannelID,
                                //SeriesTimerId = item.ScheduleID,
                                //ProgramId = GenerateProgramID(item.ID, channelId),
                                ChannelName = item.ChannelName,
                                Overview = item.VideoInfo.ShortDesc,
                                Url = item.Url,
                                StartDate = streamSourceInfo.StartDate,
                                EndDate = streamSourceInfo.EndDate,
                                HasImage = !string.IsNullOrEmpty(item.Thumbnail),
                                ImageUrl = item.Thumbnail,
                                //ImagePath = item.VideoInfo.Image,
                                IsRepeat = item.VideoInfo.Repeat,
                                IsPremiere = item.VideoInfo.Premiere,
                                IsHD = item.VideoInfo.Hdtv,
                                IsMovie = item.VideoInfo.CatMovie,
                                IsNews = item.VideoInfo.CatNews,
                                //IsSeries = item.VideoInfo.IsVideoSeries(),
                                IsSeries = item.ScheduleSeries,
                                IsSports = item.VideoInfo.CatSports,
                                IsKids = item.VideoInfo.CatKids,
                                Status = ChannelHelper.GetRecordingStatusType(item.State),
                                MediaSourceInfo = mediaSourceInfo,
                                EpisodeNumber = (int)item.VideoInfo.EpisodeNum,
                                SeasonNumber = (int)item.VideoInfo.SeasonNum,
                                Year = (int)item.VideoInfo.Year,
                                CommunityRating = item.VideoInfo.StarsNum
                            };

                            if (!string.IsNullOrEmpty(item.VideoInfo.Name))
                            {
                                recordingInfo.Name = item.VideoInfo.Name;
                            }

                            if (!string.IsNullOrEmpty(item.VideoInfo.Subname))
                            {
                                recordingInfo.EpisodeTitle = item.VideoInfo.Subname;
                            }

                            if (!string.IsNullOrEmpty(item.VideoInfo.Categories))
                            {
                                recordingInfo.Genres = new List<string>(item.VideoInfo.Categories.ToString().Split('/'));
                            }
                            recordingInfos.Add(recordingInfo);
                        }
                        catch (Exception ex)
                        {
                            this.DVBLogicPlugin.Logger.ErrorException("Get recordings Error : {0}", ex, item.ObjectId);
                        }
                    }
                    this._AllRecordingCache.SetCache(this.Client.TunerInfo.Id, cache, recordingInfos);
                    this.DVBLogicPlugin.Logger.Info("Recordings saved to cache");
                }
            }
            return recordingInfos;
        }

        /// <summary> Gets recordings asynchronous. </summary>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns>
        /// The asynchronous result that yields an enumerator that allows foreach to be used to process
        /// the recordings asynchronous in this collection. </returns>
        public async Task<IEnumerable<RecordingInfo>> GetRecordingsAsync(CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return new List<RecordingInfo>();

            DataCache<RecordingInfo> cache = this._RecordingCache.GetCache(this.Client.TunerInfo.Id);

            if (cache?.IsValid == true)
            {
                this.DVBLogicPlugin.Logger.Info("Recordings from cache: {0}", cache.Data.Count);
                return cache.Data;
            }

            this.DVBLogicPlugin.Logger.Info("Get Recordings");

            var recordingInfos = new List<RecordingInfo>();
            var result = await this.Client.GetObjectsRecordingsByNameAsync(cancellationToken).ConfigureAwait(false);
            if (result != null)
            {
                if (result.Items?.RecordedTv.Count > 0)
                {
                    this.DVBLogicPlugin.Logger.Info("Recordings found on server: {0}", result.Items.RecordedTv.Count);
                    foreach (var item in result.Items.RecordedTv)
                    {
                        try
                        {
                            var recordingInfo = new RecordingInfo
                            {
                                Id = item.ObjectId,
                                ChannelId = item.ChannelID,
                                SeriesTimerId = item.ScheduleID,
                                //ProgramId = GenerateProgramID(item.ID, channelId),
                                Overview = item.VideoInfo.ShortDesc,
                                //Url = item.Url,
                                StartDate = item.VideoInfo.GetStarDateOffset(),
                                EndDate = item.VideoInfo.GetEndDateOffset(),
                                HasImage = !string.IsNullOrEmpty(item.Thumbnail),
                                ImageUrl = item.Thumbnail,
                                IsRepeat = item.VideoInfo.Repeat,
                                IsPremiere = item.VideoInfo.Premiere,
                                IsHD = item.VideoInfo.Hdtv,
                                IsMovie = item.VideoInfo.CatMovie,
                                IsNews = item.VideoInfo.CatNews,
                                //IsSeries = item.VideoInfo.IsVideoSeries(),
                                IsSeries = item.ScheduleSeries,
                                IsSports = item.VideoInfo.CatSports,
                                IsKids = item.VideoInfo.CatKids,
                                Status = ChannelHelper.GetRecordingStatusType(item.State)
                            };

                            if (!string.IsNullOrEmpty(item.VideoInfo.Name))
                            {
                                recordingInfo.Name = item.VideoInfo.Name;
                            }

                            if (!string.IsNullOrEmpty(item.VideoInfo.Subname))
                            {
                                recordingInfo.EpisodeTitle = item.VideoInfo.Subname;
                            }

                            if (!string.IsNullOrEmpty(item.VideoInfo.Categories))
                            {
                                recordingInfo.Genres = new List<string>(item.VideoInfo.Categories.ToString().Split('/'));
                            }
                            recordingInfos.Add(recordingInfo);
                        }
                        catch (Exception ex)
                        {
                            this.DVBLogicPlugin.Logger.ErrorException("Get recordings Error : {0}", ex, item.ObjectId);
                        }
                    }
                    this._RecordingCache.SetCache(this.Client.TunerInfo.Id, cache, recordingInfos);
                    this.DVBLogicPlugin.Logger.Info("Recordings saved to cache");
                }
            }
            return recordingInfos;
        }

        /// <summary> Deletes the recording asynchronous. </summary>
        /// <param name="recordingId"> Identifier for the recording. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> The asynchronous result. </returns>
        public async Task DeleteRecordingAsync(string recordingId, CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return;

            this.DVBLogicPlugin.Logger.Info("Delete Recording : {0}", recordingId);
            var request = new ObjectRemoverRequest(recordingId);
            var result = await this.Client.RemoveObjectAsync(request, cancellationToken).ConfigureAwait(false);
            if (result != null)
            {
                this.DVBLogicPlugin.Logger.Info("Successful deleted the recording for recordingId: {0}", recordingId);
                this._RecordingCache.ClearCache(this.Client.TunerInfo.Id);
            }
            else
            {
                this.DVBLogicPlugin.Logger.Error($"It's not possible to delete the recording with recordingId: {0} on the {this.Client.Name} Server", recordingId);
            }
        }

        /// <summary> Gets recording stream media sources. </summary>
        /// <param name="recordingId"> Identifier for the recording. </param>
        /// <returns> The recording stream media sources. </returns>
        public List<MediaSourceInfo> GetRecordingStreamMediaSources(string recordingId)
        {
            if (!this.Client.Enable) return new List<MediaSourceInfo>();

            return this.GetStreamMediaSources("Recording", recordingId);
        }

        /// <summary> Gets recording stream asynchronous. </summary>
        /// <exception cref="ResourceNotFoundException"> Thrown when a Resource Not Found error condition
        /// occurs. </exception>
        /// <param name="recordingId"> Identifier for the recording. </param>
        /// <param name="mediaSourceId"> Identifier for the media source. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> The asynchronous result that yields the recording stream asynchronous. </returns>
        public async Task<MediaSourceInfo> GetRecordingStreamAsync(string recordingId, string mediaSourceId, CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return null;

            this.DVBLogicPlugin.Logger.Info("Get Recording Stream : {0}", recordingId);
            try
            {
                //bool directStream = Client.Config.DirectStreamRequest;
                var streamSourceInfo = TranscoderHelper.GeStreamSource(this.Client.Config.Transcode ? mediaSourceId : this._DefaultProfile.ToString(), this._DirectStream, this.Client.Config.EnableTimeshift, this.Client.Config.Transcode);
                if (streamSourceInfo == null)
                {
                    streamSourceInfo = TranscoderHelper.GeStreamSource(this._DefaultProfile, this._DirectStream, this.Client.Config.EnableTimeshift, this.Client.Config.Transcode);
                }

                if (streamSourceInfo != null)
                {
                    MediaSourceInfo mediaSourceInfo = this.GetMediaSource(streamSourceInfo);
                    var result = await this.Client.GetObjectItemRecordedTVAsync(recordingId, cancellationToken).ConfigureAwait(false);

                    if (result?.Count > 0)
                    {
                        var item = result.FirstOrDefault();
                        string sessionId = streamSourceInfo.Id;
                        string url = item.Url;
                        this.DVBLogicPlugin.Logger.Info("Recording stream url: {0}", url);

                        mediaSourceInfo.Size = item.Size;
                        if (item.VideoInfo?.Duration > 0)
                            mediaSourceInfo.RunTimeTicks = TimeSpan.FromSeconds(item.VideoInfo.Duration).Ticks;

                        mediaSourceInfo.Id = sessionId;
                        mediaSourceInfo.Path = url;
                        mediaSourceInfo.IsInfiniteStream = false;

                        this.Client.SessionManager[sessionId] = new TSoft.TVServer.Entities.Session
                        {
                            SessionId = sessionId,
                            StreamSource = EnumStreamSourceType.RECORDING,
                            StreamId = recordingId,
                            StreamUrl = url,
                            StreamStartTime = DateTime.Now,
                            StreamOpen = true
                        };

                        return mediaSourceInfo;
                    }
                    else
                    {
                        throw new ResourceNotFoundException(string.Format("Could not get recording {0}", recordingId));
                    }
                }
                else
                {
                    throw new ResourceNotFoundException(string.Format("Could not get recording {0}", recordingId));
                }
            }
            catch (Exception ex)
            {
                this.DVBLogicPlugin.Logger.ErrorException("Get recording async failed", ex);
            }

            throw new ResourceNotFoundException(string.Format("Could not stream recording {0}", recordingId));
        }

        #endregion

        #region [Schedules]

        /// <summary> Gets timers asynchronous. </summary>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns>
        /// The asynchronous result that yields an enumerator that allows foreach to be used to process
        /// the timers asynchronous in this collection. </returns>
        public async Task<IEnumerable<TimerInfo>> GetTimersAsync(CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return new List<TimerInfo>();

            DataCache<TimerInfo> cache = this._TimersCache.GetCache(this.Client.TunerInfo.Id);

            if (cache?.IsValid == true)
            {
                this.DVBLogicPlugin.Logger.Info("Timers from cache: {0}", cache.Data.Count);
                return cache.Data;
            }

            this.DVBLogicPlugin.Logger.Info("Get Timers");
            var timerInfos = new List<TimerInfo>();
            var result = await this.Client.GetRecordingsAsync(cancellationToken).ConfigureAwait(false);
            if (result != null)
            {
                var padding = await this.GetRecordingPadding(cancellationToken, false, true);

                this.DVBLogicPlugin.Logger.Info("Timers found on server: {0}", result.Items.Count);
                foreach (Recording item in result.Items)
                {
                    int? year = (int)item.Program.Year;
                    if (year <= 0)
                        year = null;

                    int? seasonNumber = (int)item.Program.SeasonNum;
                    if (seasonNumber <= 0)
                        seasonNumber = null;

                    int? episodeNum = (int)item.Program.EpisodeNum;
                    if (episodeNum <= 0)
                        episodeNum = null;

                    var recordingInfo = new TimerInfo
                    {
                        Id = item.RecordingID,
                        ChannelId = item.ChannelID,
                        ProgramId = ChannelHelper.GenerateProgramID(item.Program.ProgramID, item.ChannelID),
                        ShowId = item.ScheduleID,
                        Overview = item.Program.ShortDesc,
                        StartDate = item.Program.GetStarDateOffset(),
                        EndDate = item.Program.GetEndDateOffset(),
                        IsPostPaddingRequired = false,
                        IsPrePaddingRequired = false,
                        PrePaddingSeconds = padding.PrePaddingSeconds,
                        PostPaddingSeconds = padding.PostPaddingSeconds,
                        EpisodeNumber = episodeNum,
                        SeasonNumber = seasonNumber,
                        ProductionYear = year,
                        IsRepeat = item.Program.Repeat,
                        IsMovie = item.Program.CatMovie,
                        IsSeries = item.Program.IsSeries,
                        Tags = this.GetProgramTags(item.Program)
                    };

                    if (item.Program.IsRepeatRecord)
                        recordingInfo.SeriesTimerId = item.ScheduleID;

                    if (!string.IsNullOrEmpty(item.Program.Name))
                    {
                        recordingInfo.Name = item.Program.Name;
                    }

                    timerInfos.Add(recordingInfo);
                }
                this._TimersCache.SetCache(this.Client.TunerInfo.Id, cache, timerInfos);
                this.DVBLogicPlugin.Logger.Info("Timers saved to cache");
            }
            return timerInfos;
        }

        /// <summary> Creates timer asynchronous. </summary>
        /// <param name="info"> The information. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> The asynchronous result. </returns>
        public async Task CreateTimerAsync(TimerInfo info, CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return;

            this.DVBLogicPlugin.Logger.Info("Create Timer");

            var padding = await this.GetRecordingPadding(cancellationToken, true);

            var request = new ScheduleAddRequest
            {
                ForceAdd = false,
                MargineBefore = padding.PaddingFromTimer ? info.PostPaddingSeconds : padding.PrePaddingSeconds,
                MargineAfter = padding.PaddingFromTimer ? info.PrePaddingSeconds : padding.PostPaddingSeconds,
                Active = true,
                Priority = (EnumSchedulePriority)info.Priority
            };

            string programID = ChannelHelper.GetProgramID(info.ProgramId);
            request.ByEpg = new ByEpgSchedule
            {
                ChannelID = info.ChannelId,
                ProgramID = programID,
                Repeat = info.IsRepeat,
                NewOnly = false
            };

            var result = await this.Client.AddScheduleAsync(request, cancellationToken).ConfigureAwait(false);
            if (result != null)
            {
                this.DVBLogicPlugin.Logger.Info("Successful Create timer for ChannelId: {0} & Name: {1}", info.ChannelId, info.Name);
                this._TimersCache.ClearCache(this.Client.TunerInfo.Id);
            }
            else
            {
                this.DVBLogicPlugin.Logger.Error("Failed to create timer for ChannelId: {0} & Name: {1}", info.ChannelId, info.Name);
            }
        }

        /// <summary> Updates the timer asynchronous. </summary>
        /// <param name="info"> The information. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> The asynchronous result. </returns>
        public async Task UpdateTimerAsync(TimerInfo info, CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return;

            this.DVBLogicPlugin.Logger.Info("Update Timer");
            var request = new ScheduleUpdater
            {
                ScheduleID = info.Id,
                MargineBefore = this.DVBLogicPlugin.Config.ServerConfig.UseServerRecordingPaddings ? -1 : info.PrePaddingSeconds,
                MargineAfter = this.DVBLogicPlugin.Config.ServerConfig.UseServerRecordingPaddings ? -1 : info.PostPaddingSeconds
            };
            var result = await this.Client.UpdateScheduleAsync(request, cancellationToken).ConfigureAwait(false);
            if (result != null)
            {
                this.DVBLogicPlugin.Logger.Info("Successful Update timer for ChannelId: {0} & Name: {1}", info.ChannelId, info.Name);
                this._TimersCache.ClearCache(this.Client.TunerInfo.Id);
            }
            else
            {
                this.DVBLogicPlugin.Logger.Error("Update timer async for ChannelId: {0} & Name: {1}", info.ChannelId, info.Name);
            }
        }

        /// <summary> Cancel timer asynchronous. </summary>
        /// <param name="timerId"> Identifier for the timer. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> The asynchronous result. </returns>
        public async Task CancelTimerAsync(string timerId, CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return;

            this.DVBLogicPlugin.Logger.Info("Cancel Timer");
            var request = new RecordingRemover(timerId);
            var result = await this.Client.RemoveRecordingAsync(request, cancellationToken).ConfigureAwait(false);
            if (result != null)
            {
                this.DVBLogicPlugin.Logger.Info("Successful cancelled the pending Recording for recordingId: {0}", timerId);
                this._TimersCache.ClearCache(this.Client.TunerInfo.Id);
            }
            else
            {
                this.DVBLogicPlugin.Logger.Error($"It's not possible to cancel the pending recording with recordingId: {0} on the {this.Client.Name} Server", timerId);
            }
        }

        /// <summary> Gets new timer defaults asynchronous. </summary>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <param name="program"> (Optional) The program. </param>
        /// <returns> The asynchronous result that yields the new timer defaults asynchronous. </returns>
        public async Task<SeriesTimerInfo> GetNewTimerDefaultsAsync(CancellationToken cancellationToken, ProgramInfo program = null)
        {
            if (!this.Client.Enable) return null;

            this.DVBLogicPlugin.Logger.Info("Get New Timer Defaults");
            var defaultTimer = new SeriesTimerInfo();
            var result = await this.Client.GetRecordingSettingsAsync(cancellationToken).ConfigureAwait(false);
            if (result != null)
            {
                defaultTimer.PostPaddingSeconds = result.BeforeMargin;
                defaultTimer.PrePaddingSeconds = result.AfterMargin;
                defaultTimer.IsPostPaddingRequired = false;
                defaultTimer.IsPrePaddingRequired = false;
            }
            return defaultTimer;
        }

        #endregion

        #region [Series]
        /// <summary> Gets series timers asynchronous. </summary>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns>
        /// The asynchronous result that yields an enumerator that allows foreach to be used to process
        /// the series timers asynchronous in this collection. </returns>
        public async Task<IEnumerable<SeriesTimerInfo>> GetSeriesTimersAsync(CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return new List<SeriesTimerInfo>();

            DataCache<SeriesTimerInfo> cache = this._SeriesTimersCache.GetCache(this.Client.TunerInfo.Id);

            if (cache?.IsValid == true)
            {
                this.DVBLogicPlugin.Logger.Info("Series Timers from cache: {0}", cache.Data.Count);
                return cache.Data;
            }

            this.DVBLogicPlugin.Logger.Info("Get Series Timers");

            var timerInfos = new List<SeriesTimerInfo>();
            var result = await this.Client.GetSchedulesAsync(cancellationToken).ConfigureAwait(false);
            if (result != null)
            {
                var padding = await this.GetRecordingPadding(cancellationToken, false, false);

                this.DVBLogicPlugin.Logger.Info("Series Timers found on server: {0}", result.Items.Count);
                foreach (var item in result.Items.Where(n => n.ByEpg.Repeat == true))
                {
                    var recordingInfo = new SeriesTimerInfo
                    {
                        Id = item.ScheduleID,
                        ChannelId = item.ByEpg.ChannelID,
                        ProgramId = ChannelHelper.GenerateProgramID(item.ByEpg.Program.ProgramID, item.ByEpg.ChannelID),
                        Overview = item.ByEpg.Program.ShortDesc,
                        StartDate = item.ByEpg.Program.GetStarDateOffset(),
                        EndDate = item.ByEpg.Program.GetEndDateOffset(),
                        IsPostPaddingRequired = false,
                        IsPrePaddingRequired = false,
                        RecordNewOnly = item.ByEpg.NewOnly,
                        RecordAnyTime = item.ByEpg.RecordSeriesAnytime,
                        PrePaddingSeconds = padding.PaddingFromTimer ? item.MargineBefore : padding.PrePaddingSeconds,
                        PostPaddingSeconds = padding.PaddingFromTimer ? item.MargineAfter : padding.PostPaddingSeconds,
                        Priority = (int)item.Priority
                    };

                    if (!string.IsNullOrEmpty(item.ByEpg.Program.Name))
                    {
                        recordingInfo.Name = item.ByEpg.Program.Name;
                    }

                    timerInfos.Add(recordingInfo);
                }
                this._SeriesTimersCache.SetCache(this.Client.TunerInfo.Id, cache, timerInfos);
                this.DVBLogicPlugin.Logger.Info("Series Timers saved to cache");
            }

            return timerInfos;
        }

        /// <summary> Creates series timer asynchronous. </summary>
        /// <param name="info"> The information. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> The asynchronous result. </returns>
        public async Task CreateSeriesTimerAsync(SeriesTimerInfo info, CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return;

            this.DVBLogicPlugin.Logger.Info("Create Series Timer");

            var padding = await this.GetRecordingPadding(cancellationToken, true);

            var request = new ScheduleAddRequest
            {
                MargineBefore = padding.PaddingFromTimer ? info.PostPaddingSeconds : padding.PrePaddingSeconds,
                MargineAfter = padding.PaddingFromTimer ? info.PrePaddingSeconds : padding.PostPaddingSeconds,
                Active = true,
                Priority = (EnumSchedulePriority)info.Priority
            };

            string programID = ChannelHelper.GetProgramID(info.ProgramId);
            request.ByEpg = new ByEpgSchedule
            {
                ChannelID = info.ChannelId,
                ProgramID = programID,
                NewOnly = info.RecordNewOnly,
                RecordSeriesAnytime = info.RecordAnyTime,
                Repeat = true,
                RecordingsToKeep = info.KeepUpTo,
                StartBefore = -1,
                StartAfter = -1
            };

            var result = await this.Client.AddScheduleAsync(request, cancellationToken).ConfigureAwait(false);
            if (result != null)
                this.DVBLogicPlugin.Logger.Info("Successful Create series timer for ChannelId: {0} & Name: {1}", info.ChannelId, info.Name);
            else
                this.DVBLogicPlugin.Logger.Error("Failed to create series timer for ChannelId: {0} & Name: {1}", info.ChannelId, info.Name);
        }

        /// <summary> Updates the series timer asynchronous. </summary>
        /// <param name="info"> The information. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> The asynchronous result. </returns>
        public async Task UpdateSeriesTimerAsync(SeriesTimerInfo info, CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return;

            this.DVBLogicPlugin.Logger.Info("Update Series Timer");
            var request = new ScheduleUpdater
            {
                ScheduleID = info.Id,
                NewOnly = info.RecordNewOnly,
                RecordSeriesAnytime = info.RecordAnyTime,
                MargineBefore = this.DVBLogicPlugin.Config.ServerConfig.UseServerRecordingPaddings ? -1 : info.PrePaddingSeconds,
                MargineAfter = this.DVBLogicPlugin.Config.ServerConfig.UseServerRecordingPaddings ? -1 : info.PostPaddingSeconds
            };

            var result = await this.Client.UpdateScheduleAsync(request, cancellationToken).ConfigureAwait(false);
            if (result != null)
                this.DVBLogicPlugin.Logger.Info("Successful pdate series timer for ChannelId: {0} & Name: {1}", info.ChannelId, info.Name);
            else
                this.DVBLogicPlugin.Logger.Error("Failed to Update series timer for ChannelId: {0} & Name: {1}", info.ChannelId, info.Name);
        }

        /// <summary> Cancel series timer asynchronous. </summary>
        /// <param name="timerId"> Identifier for the timer. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> The asynchronous result. </returns>
        public async Task CancelSeriesTimerAsync(string timerId, CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return;

            this.DVBLogicPlugin.Logger.Info("Cancel Series Timer");
            var request = new ScheduleRemover(timerId);

            var result = await this.Client.RemoveScheduleAsync(request, cancellationToken).ConfigureAwait(false);
            if (result != null)
            {
                this.DVBLogicPlugin.Logger.Info("Successful cancelled the pending SeriesRecording for recordingId: {0}", timerId);
            }
            else
            {
                this.DVBLogicPlugin.Logger.Error($"It's not possible to cancel the pending SeriesRecording with recordingId: {0} on the {this.Client.Name} Server", timerId);
            }
        }

        #endregion

        #region [Live TV]
        /// <summary> Gets channel stream asynchronous. </summary>
        /// <exception cref="ResourceNotFoundException"> Thrown when a Resource Not Found error condition
        /// occurs. </exception>
        /// <param name="channelId"> Identifier for the channel. </param>
        /// <param name="mediaSourceId"> Identifier for the media source. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> The asynchronous result that yields the channel stream asynchronous. </returns>
        public async Task<MediaSourceInfo> GetChannelStreamAsync(string channelId, string mediaSourceId, CancellationToken cancellationToken)
        {
            if (!this.Client.Enable) return null;

            this.DVBLogicPlugin.Logger.Info("Get Channel Stream : {0}", channelId);
            try
            {
                this._DirectStream = this.Client.Config.DirectStreamRequest;
                //var channels = await this.GetChannelsAsync(cancellationToken, this._LogosPath);
                //var channel = channels.FirstOrDefault(i => string.Equals(i.Id, channelId, StringComparison.OrdinalIgnoreCase));
                var streamSourceInfo = TranscoderHelper.GeStreamSource(this.Client.Config.Transcode ? mediaSourceId : this._DefaultProfile.ToString(), this._DirectStream, this.Client.Config.EnableTimeshift, this.Client.Config.Transcode);
                if (streamSourceInfo == null)
                {
                    streamSourceInfo = TranscoderHelper.GeStreamSource(this._DefaultProfile, this._DirectStream, this.Client.Config.EnableTimeshift, this.Client.Config.Transcode);
                }

                if (streamSourceInfo != null)
                {
                    var streamOpen = false;
                    long channelHandle = -1;

                    this.DVBLogicPlugin.Logger.Info("Stream Source Info Name: {0}", streamSourceInfo.Name);
                    MediaSourceInfo mediaSourceInfo = this.GetMediaSource(streamSourceInfo);

                    string url = string.Empty;
                    string sessionId = streamSourceInfo.Id;

                    if (streamSourceInfo.DirectStreamRequest == true)
                    {
                        if (!streamOpen)
                        {
                            url = this.Client.GetDirectChannelStream(streamSourceInfo, sessionId, channelId);
                            streamOpen = true;
                        }
                    }
                    else
                    {
                        if (!streamOpen)
                        {
                            mediaSourceInfo.RequiresOpening = true;
                            mediaSourceInfo.RequiresClosing = true;
                            var request = this.Client.GetIndirectChannelStream(streamSourceInfo, sessionId, channelId, string.Empty, this.Client.Config.LiveTVDuration);

                            var result = await this.Client.PlayChannelAsync(request, cancellationToken).ConfigureAwait(false);

                            if (result != null)
                            {
                                streamOpen = true;
                                url = result.Url;
                                channelHandle = result.ChannelHandle;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(url) && streamOpen)
                    {
                        this.DVBLogicPlugin.Logger.Info("TV {0} stream : SourceInfo - {1}, Channel : {2}, URL:{3}", streamSourceInfo.DirectStreamRequest ? "Direct" : "Indirect", streamSourceInfo.Id, channelId, url);
                        mediaSourceInfo.Id = sessionId;
                        mediaSourceInfo.Path = url;

                        this.Client.SessionManager[sessionId] = new Session
                        {
                            SessionId = sessionId,
                            StreamSource = EnumStreamSourceType.LIVETV,
                            StreamId = channelId,
                            StreamUrl = url,
                            StreamStartTime = DateTime.Now,
                            StreamHandle = channelHandle,
                            StreamOpen = true
                        };

                        return mediaSourceInfo;
                    }
                    else
                    {
                        var request = new StopChannelRequest(sessionId);
                        var result = await this.Client.StopChannelAsync(request, cancellationToken).ConfigureAwait(false);
                        throw new ResourceNotFoundException(string.Format("Could not stream channel {0}", channelId));
                    }
                }
                else
                {
                    throw new ResourceNotFoundException(string.Format("Could not stream channel {0}", channelId));
                }
            }
            catch (Exception ex)
            {
                this.DVBLogicPlugin.Logger.ErrorException("Get channel stream failed for ChannelId: {0}", ex, channelId);
            }

            throw new ResourceNotFoundException(string.Format("Could not stream channel {0}", channelId));
        }

        /// <summary> Gets channel stream media sources. </summary>
        /// <param name="channelId"> Identifier for the channel. </param>
        /// <returns> The channel stream media sources. </returns>
        public List<MediaSourceInfo> GetChannelStreamMediaSources(string channelId)
        {
            if (!this.Client.Enable) return new List<MediaSourceInfo>();

            return this.GetStreamMediaSources("Channel", channelId);
        }

        /// <summary> Closes live stream asynchronous. </summary>
        /// <param name="id"> The identifier. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <param name="channelHandle"> (Optional) True to channel handle. </param>
        /// <returns> The asynchronous result. </returns>
        public async Task CloseLiveStreamAsync(string id, CancellationToken cancellationToken, bool channelHandle = false)
        {
            if (!this.Client.Enable) return;

            this.DVBLogicPlugin.Logger.Info("Close live Stream");
            var streamClosed = false;
            try
            {
                StopChannelRequest request;
                if (channelHandle)
                {
                    var session = this.DVBLogicPlugin.TVProxy.Client.SessionManager.GetSession(id);
                    if (session?.StreamHandle > 0)
                    {
                        request = new StopChannelRequest(session.StreamHandle);
                    }
                    else
                    {
                        request = new StopChannelRequest(id);
                    }
                }
                else
                {
                    request = new StopChannelRequest(id);
                }

                var result = await this.Client.StopChannelAsync(request, cancellationToken).ConfigureAwait(false);

                if (result != null)
                    streamClosed = true;
            }
            catch (Exception ex)
            {
                this.DVBLogicPlugin.Logger.ErrorException("Close live stream async failed for the stream with ChannelId: {0}", ex, id);
            }

            if (streamClosed)
            {
                var session = this.DVBLogicPlugin.TVProxy.Client.SessionManager.GetSession(id);
                if (session != null)
                {
                    session.StreamOpen = false;
                }
            }
        }

        #endregion

        #region [Helpers]

        /// <summary> Gets stream media sources. </summary>
        /// <exception cref="ResourceNotFoundException"> Thrown when a Resource Not Found error condition
        /// occurs. </exception>
        /// <param name="type"> The type. </param>
        /// <param name="id"> The identifier. </param>
        /// <returns> The stream media sources. </returns>
        private List<MediaSourceInfo> GetStreamMediaSources(string type, string id)
        {
            this.DVBLogicPlugin.Logger.Info("Get stream MediaSource for {0}: {1}", type, id);
            try
            {
                //bool directStream = Client.Config.DirectStreamRequest;
                var mediaSource = new List<MediaSourceInfo>();
                var streamSourceInfos = TranscoderHelper.GeStreamSources(this._DirectStream, this.Client.Config.EnableTimeshift);
                this.DVBLogicPlugin.Logger.Info("Stream MediaSources found: {0}", streamSourceInfos.Count);
                foreach (StreamSourceInfo source in streamSourceInfos)
                {
                    MediaSourceInfo info = this.GetMediaSource(source);
                    if (info != null)
                        mediaSource.Add(info);
                }

                //string jsonSerializerSerializeToString = _jsonSerializer.SerializeToString(mediaSource);
                //Plugin.Logger.Info("MediaSourceInfos: {0}", jsonSerializerSerializeToString);
                return mediaSource;
            }
            catch (Exception ex)
            {
                this.DVBLogicPlugin.Logger.ErrorException("Get stream MediaSourceInfo failed for {0}: {1}", ex, type, id);
            }

            throw new ResourceNotFoundException(string.Format("Get stream MediaSourceInfo failed for {0}: {1}", type, id));
        }

        /// <summary> Gets media source. </summary>
        /// <param name="streamSourceInfo"> Information describing the stream source. </param>
        /// <returns> The media source. </returns>
        private MediaSourceInfo GetMediaSource(StreamSourceInfo streamSourceInfo, bool setMediaStreams = false)
        {
            try
            {
                var config = this.DVBLogicPlugin.Config;
                var videoCodec = streamSourceInfo.Transcoder.VideoCodec;
                bool inProgress = streamSourceInfo.state == EnumState.RTVS_IN_PROGRESS;
                var sourceInfo = new MediaSourceInfo
                {
                    //Path = url,
                    Protocol = MediaProtocol.Http,
                    RequiresOpening = false,
                    RequiresClosing = false,
                    //Container = streamSourceInfo.Transcoder.Container,
                    Id = streamSourceInfo.Name,
                    Name = streamSourceInfo.Name,
                    SupportsDirectPlay = true,
                    SupportsDirectStream = true,
                    SupportsTranscoding = true,
                    Type = MediaSourceType.Default,
                    //Bitrate = streamSourceInfo.Transcoder.Bitrate,
                    //IsRemote = true,
                    IsInfiniteStream = inProgress,
                    //RunTimeTicks = (streamSourceInfo.EndDate - streamSourceInfo.StartDate).Ticks,
                };

                if (setMediaStreams)
                {
                    sourceInfo.MediaStreams = new List<MediaStream>
                    {
                        new MediaStream
                        {
                            Type = MediaStreamType.Video,
							// Set the index to -1 because we don't know the exact index of the video stream within the container
							Index = -1,
                            IsInterlaced = streamSourceInfo.Transcoder.IsInterlaced,
                            //Codec = streamSourceInfo.Transcoder.VideoCodec,
                            Codec = videoCodec,
                            Width = streamSourceInfo.Transcoder.Width,
                            Height = streamSourceInfo.Transcoder.Height,
                            BitRate = streamSourceInfo.Transcoder.VideoBitrate,
							//Profile = streamSourceInfo.Transcoder.Profile,
							//Level = streamSourceInfo.Transcoder.Level,
							NalLengthSize = null
                        },
                        new MediaStream
                        {
                            Type = MediaStreamType.Audio,
							// Set the index to -1 because we don't know the exact index of the audio stream within the container
							Index = -1,
                            Codec = streamSourceInfo.Transcoder.AudioCodec,
                            BitRate = streamSourceInfo.Transcoder.AudioBitrate,
                            IsDefault = true
                        }
                    };
                }

                return sourceInfo;
            }
            catch (Exception ex)
            {
                this.DVBLogicPlugin.Logger.ErrorException("Get stream MediaSourceInfo failed : {0}", ex, streamSourceInfo.Profile);
            }

            return null;
        }

        /// <summary> Gets recording padding. </summary>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <param name="create">            (Optional) True to create. </param>
        /// <param name="force">             (Optional) True to force. </param>
        /// <returns> An asynchronous result that yields the recording padding. </returns>
        private async Task<(int PrePaddingSeconds, int PostPaddingSeconds, bool PaddingFromTimer)> GetRecordingPadding(
            CancellationToken cancellationToken, bool create = false, bool force = false)
        {
            var internalPrePaddingSeconds = -1;
            var internalPostPaddingSeconds = -1;
            bool getPaddingFromTimer = true;

            if ((((this.Client.Status.Version < 7) || create) && this.DVBLogicPlugin.Config.ServerConfig.UseServerRecordingPaddings) || force)
            {
                RecordingSettings rc = await this.Client.GetRecordingSettingsAsync(cancellationToken).ConfigureAwait(false);
                if (rc != null)
                {
                    internalPrePaddingSeconds = rc.BeforeMargin;
                    internalPostPaddingSeconds = rc.AfterMargin;
                }
                getPaddingFromTimer = false;
            }

            return (internalPrePaddingSeconds, internalPostPaddingSeconds, getPaddingFromTimer);
        }

        /// <summary> Gets program tags. </summary>
        /// <param name="program"> The program. </param>
        /// <returns> An array of string. </returns>
        private string[] GetProgramTags(Program program)
        {
            List<string> tags = new List<string>();
            if (program.IsSeries) tags.Add("Series");
            if (program.CatNews) tags.Add("News");
            if (program.Hdtv) tags.Add("Hdtv");
            if (program.Premiere) tags.Add("Premiere");
            if (program.CatAction) tags.Add("Action");
            if (program.CatComedy) tags.Add("Comedy");
            if (program.CatDocumentary) tags.Add("Documentary");
            if (program.CatDrama) tags.Add("Drama");
            if (program.CatEducational) tags.Add("Educational");
            if (program.CatHorror) tags.Add("Horror");
            if (program.CatKids) tags.Add("Kids");
            if (program.CatMovie) tags.Add("Movie");
            if (program.CatMusic) tags.Add("Music");
            if (program.CatReality) tags.Add("Reality");
            if (program.CatRomance) tags.Add("Romance");
            if (program.CatScifi) tags.Add("Scifi");
            if (program.CatScifi) tags.Add("Sci-fi");
            if (program.CatSerial) tags.Add("Serial");
            if (program.CatSoap) tags.Add("Soap");
            if (program.CatSpecial) tags.Add("Special");
            if (program.CatSports) tags.Add("Sports");
            if (program.CatThriller) tags.Add("Thriller");
            if (program.CatAdult) tags.Add("Adult");
            return tags.ToArray();
        }
        #endregion
    }
}
