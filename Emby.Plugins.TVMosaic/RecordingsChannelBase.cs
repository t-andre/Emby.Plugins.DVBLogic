// <copyright file="RecordingsChannelBase.cs" >
// Copyright (c) 2018 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares</author>
// <date>28.08.2018</date>
// <summary>Implements the recordings channel base class</summary>
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Channels;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Channels;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.Controller.LiveTv;
using System.Linq;
using MediaBrowser.Common.Extensions;
using MediaBrowser.Model.Dto;
using System.Globalization;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.LiveTv;

namespace Emby.Plugins.DVBLogic
{
    /// <summary> The recordings channel base. </summary>
    /// <seealso cref="T:MediaBrowser.Controller.Channels.IChannel"/>
    /// <seealso cref="T:MediaBrowser.Controller.Channels.IHasCacheKey"/>
    /// <seealso cref="T:MediaBrowser.Controller.Channels.ISupportsDelete"/>
    /// <seealso cref="T:MediaBrowser.Controller.Channels.ISupportsLatestMedia"/>
    /// <seealso cref="T:MediaBrowser.Controller.Channels.ISupportsMediaProbe"/>
    /// <seealso cref="T:MediaBrowser.Controller.Channels.IHasFolderAttributes"/>
    public abstract class RecordingsChannelBase : IChannel, IHasCacheKey, ISupportsDelete, ISupportsLatestMedia, ISupportsMediaProbe, IHasFolderAttributes, IHasChangeEvent, IDisposable
    {
        #region [Constructors]
        /// <summary> Initializes a new instance of the Emby.Plugins.DVBLink.RecordingsChannelBase class. </summary>
        /// <param name="liveTvManager"> Manager for live TV. </param>
        public RecordingsChannelBase(ILiveTvManager liveTvManager)
        {
            _liveTvManager = liveTvManager;
            var interval = TimeSpan.FromMinutes(15);
            _updateTimer = new Timer(OnUpdateTimerCallback, null, interval, interval);
        }
        #endregion

        #region [Fields]
        /// <summary> The update timer. </summary>
        private Timer _updateTimer;

        /// <summary> Manager for live TV. </summary>
        public ILiveTvManager _liveTvManager;
        #endregion

        #region [Events]
        /// <summary> Occurs when Content Changed. </summary>
        public event EventHandler ContentChanged;
        #endregion

        #region [Public properties]
        /// <summary> Gets the attributes. </summary>
        /// <value> The attributes. </value>
        public string[] Attributes
        {
            get
            {
                return new[] { "Recordings" };
            }
        }

        /// <summary> Gets the data version. </summary>
        /// <value> The data version. </value>
        public abstract string DataVersion { get; }

        /// <summary> Gets the description. </summary>
        /// <value> The description. </value>
        /// <seealso cref="P:MediaBrowser.Controller.Channels.IChannel.Description"/>
        public abstract string Description { get; }

        /// <summary> Gets URL of the home page. </summary>
        /// <value> The home page URL. </value>
        public abstract string HomePageUrl { get; }

        /// <summary> Gets the name. </summary>
        /// <value> The name. </value>
        /// <seealso cref="P:MediaBrowser.Controller.Channels.IChannel.Name"/>
        public abstract string Name { get; }

        /// <summary> Gets the parental rating. </summary>
        /// <value> The parental rating. </value>
        /// <seealso cref="P:MediaBrowser.Controller.Channels.IChannel.ParentalRating"/>
        public ChannelParentalRating ParentalRating
        {
            get { return ChannelParentalRating.GeneralAudience; }
        }
        #endregion

        #region [Public methods]
        /// <summary> Determine if we can delete. </summary>
        /// <param name="item"> The item. </param>
        /// <returns> True if we can delete, false if not. </returns>
        public bool CanDelete(BaseItem item)
        {
            return !item.IsFolder;
        }

        /// <summary> Deletes the item. </summary>
        /// <param name="id">                The identifier. </param>
        /// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
        /// <returns> An asynchronous result. </returns>
        public Task DeleteItem(string id, CancellationToken cancellationToken)
        {
            return GetService().DeleteRecordingAsync(id, cancellationToken);
        }

        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
            if (_updateTimer != null)
            {
                _updateTimer.Dispose();
                _updateTimer = null;
            }
        }

        /// <summary> Gets cache key. </summary>
        /// <param name="userId"> Identifier for the user. </param>
        /// <returns> The cache key. </returns>
        /// <seealso cref="M:MediaBrowser.Controller.Channels.IHasCacheKey.GetCacheKey(string)"/>
        public string GetCacheKey(string userId)
        {
            var now = DateTimeOffset.UtcNow;

            var values = new List<string>();

            values.Add(now.DayOfYear.ToString(CultureInfo.InvariantCulture));
            values.Add(now.Hour.ToString(CultureInfo.InvariantCulture));

            double minute = now.Minute;
            minute /= 5;

            values.Add(Math.Floor(minute).ToString(CultureInfo.InvariantCulture));

            values.Add(GetService().LastRecordingChange.Ticks.ToString(CultureInfo.InvariantCulture));

            return string.Join("-", values.ToArray());
        }

        /// <summary> Gets channel features. </summary>
        /// <returns> The channel features. </returns>
        /// <seealso cref="M:MediaBrowser.Controller.Channels.IChannel.GetChannelFeatures()"/>
        public InternalChannelFeatures GetChannelFeatures()
        {
            return new InternalChannelFeatures
            {
                ContentTypes = new List<ChannelMediaContentType>
                 {
                    ChannelMediaContentType.Movie,
                    ChannelMediaContentType.Episode,
                    ChannelMediaContentType.Clip
                 },
                MediaTypes = new List<ChannelMediaType>
                {
                    ChannelMediaType.Audio,
                    ChannelMediaType.Video
                },
                SupportsContentDownloading = true
            };
        }

        /// <summary> Gets channel image. </summary>
        /// <param name="type">              The type. </param>
        /// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
        /// <returns> An asynchronous result that yields the channel image. </returns>
        /// <seealso cref="M:MediaBrowser.Controller.Channels.IChannel.GetChannelImage(ImageType,CancellationToken)"/>
        public abstract Task<DynamicImageResponse> GetChannelImage(ImageType type, CancellationToken cancellationToken);

        /// <summary> Gets channel items. </summary>
        /// <param name="query">             The query. </param>
        /// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
        /// <returns> An asynchronous result that yields the channel items. </returns>
        /// <seealso cref="M:MediaBrowser.Controller.Channels.IChannel.GetChannelItems(InternalChannelItemQuery,CancellationToken)"/>
        public Task<ChannelItemResult> GetChannelItems(InternalChannelItemQuery query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query.FolderId))
            {
                var recordingGroups = GetRecordingGroups(query, cancellationToken);

                if (recordingGroups.Result.Items.Count == 0)
                {
                    return GetRecordingNameGroups(query, i => !i.IsSports && !i.IsNews && !i.IsMovie && !i.IsKids && !i.IsSeries, cancellationToken);
                }

                return recordingGroups;
            }

            /// <summary>
            /// Optional Latest Folders
            /// </summary>
            /// 
            /// if (string.Equals(query.FolderId, "latest", StringComparison.OrdinalIgnoreCase))
            /// {
            ///     var latestRecordings = GetChannelItems(query, i => true, cancellationToken).Result;
            ///
            ///     var latest = new ChannelItemResult()
            ///     {
            ///         Items = latestRecordings.Items.OrderByDescending(i => i.DateCreated).Take(25).ToList()
            ///     };
            ///
            ///     return Task.FromResult(latest);
            /// }

            if (string.Equals(query.FolderId, "tvshows", StringComparison.OrdinalIgnoreCase))
            {
                return GetRecordingSeriesGroups(query, cancellationToken);
            }

            if (query.FolderId.StartsWith("series_", StringComparison.OrdinalIgnoreCase))
            {
                var hash = query.FolderId.Split('_')[1];
                return GetChannelItems(query, i => i.IsSeries && string.Equals(i.Name.GetMD5().ToString("N"), hash, StringComparison.Ordinal), cancellationToken);
            }

            /// <summary>
            /// Optional Season Folders
            /// </summary>
            /// 
            /// if (query.FolderId.StartsWith("series_", StringComparison.OrdinalIgnoreCase))
            /// {
            ///     var hash = query.FolderId.Split('_')[1];
            ///
            ///     return GetRecordingSeasonGroups(query, i => i.IsSeries && string.Equals(i.Name.GetMD5().ToString("N"), hash, StringComparison.Ordinal), cancellationToken);
            /// }
            ///
            /// if (query.FolderId.Contains("_season_"))
            /// {
            ///     var name = query.FolderId.Split('_')[0];
            ///     var hash = query.FolderId.Split('_')[2];
            ///
            ///     return GetChannelItems(query, i => i.IsSeries && string.Equals(i.Name, name) && string.Equals(i.SeasonNumber.ToString().GetMD5().ToString("N"), hash, StringComparison.Ordinal), cancellationToken);
            /// }


            if (string.Equals(query.FolderId, "movies", StringComparison.OrdinalIgnoreCase))
            {
                return GetChannelItems(query, i => i.IsMovie, cancellationToken);
            }

            if (string.Equals(query.FolderId, "kids", StringComparison.OrdinalIgnoreCase))
            {
                return GetRecordingNameGroups(query, i => i.IsKids, cancellationToken);
            }

            if (string.Equals(query.FolderId, "news", StringComparison.OrdinalIgnoreCase))
            {
                return GetRecordingNameGroups(query, i => i.IsNews, cancellationToken);
            }

            if (string.Equals(query.FolderId, "sports", StringComparison.OrdinalIgnoreCase))
            {
                return GetRecordingNameGroups(query, i => i.IsSports, cancellationToken);
            }

            if (string.Equals(query.FolderId, "live", StringComparison.OrdinalIgnoreCase))
            {
                return GetRecordingNameGroups(query, i => i.IsLive, cancellationToken);
            }

            if (string.Equals(query.FolderId, "others", StringComparison.OrdinalIgnoreCase))
            {
                return GetRecordingNameGroups(query, i => !i.IsSports && !i.IsNews && !i.IsMovie && !i.IsKids && !i.IsSeries, cancellationToken);
            }

            if (query.FolderId.StartsWith("name_", StringComparison.OrdinalIgnoreCase))
            {
                var hash = query.FolderId.Split('_')[1];
                return GetChannelItems(query, i => i.Name != null && string.Equals(i.Name.GetMD5().ToString("N"), hash, StringComparison.Ordinal), cancellationToken);
            }

            var result = new ChannelItemResult()
            {
                Items = new List<ChannelItemInfo>()
            };

            return Task.FromResult(result);
        }

        /// <summary> Gets channel items. </summary>
        /// <param name="query">             The query. </param>
        /// <param name="filter">            Specifies the filter. </param>
        /// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
        /// <returns> An asynchronous result that yields the channel items. </returns>
        public async Task<ChannelItemResult> GetChannelItems(InternalChannelItemQuery query, Func<MyRecordingInfo, bool> filter, CancellationToken cancellationToken)
        {
            var service = GetService();
            var allRecordings = await service.GetAllRecordingsAsync(cancellationToken).ConfigureAwait(false);

            var result = new ChannelItemResult()
            {
                Items = new List<ChannelItemInfo>()
            };

            result.Items.AddRange(allRecordings.Where(filter).Select(ConvertToChannelItem));

            return result;
        }

        /// <summary> Gets the supported channel images in this collection. </summary>
        /// <returns> An enumerator that allows foreach to be used to process the supported channel images in this collection. </returns>
        /// <seealso cref="M:MediaBrowser.Controller.Channels.IChannel.GetSupportedChannelImages()"/>
        public IEnumerable<ImageType> GetSupportedChannelImages()
        {
            return new List<ImageType>
            {
                 ImageType.Primary,
                 ImageType.Thumb
            };
        }

        /// <summary> Query if 'userId' is enabled for. </summary>
        /// <param name="userId"> Identifier for the user. </param>
        /// <returns> True if enabled for, false if not. </returns>
        /// <seealso cref="M:MediaBrowser.Controller.Channels.IChannel.IsEnabledFor(string)"/>
        public bool IsEnabledFor(string userId)
        {
            return true;
        }

        /// <summary> Executes the content changed action. </summary>
        public void OnContentChanged()
        {
            if (ContentChanged != null)
            {
                ContentChanged(this, EventArgs.Empty);
            }
        }
        #endregion

        #region [Private methods]
        /// <summary> Converts an item to a channel item. </summary>
        /// <param name="item"> The item. </param>
        /// <returns> The given data converted to a channel item. </returns>
        private ChannelItemInfo ConvertToChannelItem(MyRecordingInfo item)
        {
            var path = string.IsNullOrEmpty(item.Path) ? item.Url : item.Path;

            var channelItem = new ChannelItemInfo
            {
                Id = item.Id,
                Name = !string.IsNullOrEmpty(item.EpisodeTitle) && (item.IsSeries || item.EpisodeNumber.HasValue && !item.IsMovie) ? item.EpisodeTitle : item.Name,
                SeriesName = !string.IsNullOrEmpty(item.EpisodeTitle) && (item.IsSeries || item.EpisodeNumber.HasValue && !item.IsMovie) ? item.Name : null,
                OriginalTitle = !string.IsNullOrEmpty(item.EpisodeTitle) && item.IsMovie ? item.EpisodeTitle : null,
                IndexNumber = item.EpisodeNumber,
                ParentIndexNumber = item.SeasonNumber,
                ProductionYear = item.Year,
                Overview = item.Overview,
                Genres = item.Genres,
                ImageUrl = item.ImageUrl,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                OfficialRating = item.OfficialRating,
                CommunityRating = item.CommunityRating,

                Type = ChannelItemType.Media,
                ContentType = item.IsMovie ? ChannelMediaContentType.Movie : (item.IsSeries || item.EpisodeNumber != null ? ChannelMediaContentType.Episode : ChannelMediaContentType.Clip),
                MediaType = item.ChannelType == ChannelType.TV ? ChannelMediaType.Video : ChannelMediaType.Audio,
                IsLiveStream = item.Status == RecordingStatus.InProgress,
                Etag = item.Status.ToString(),

                MediaSources = new List<MediaSourceInfo> { item.MediaSourceInfo },

                PremiereDate = item.OriginalAirDate,
                DateModified = item.DateLastUpdated,
            };

            return channelItem;
        }

        /// <summary> Gets recording groups. </summary>
        /// <param name="query">             The query. </param>
        /// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
        /// <returns> An asynchronous result that yields the recording groups. </returns>
        private async Task<ChannelItemResult> GetRecordingGroups(InternalChannelItemQuery query, CancellationToken cancellationToken)
        {
            var service = GetService();

            var allRecordings = await service.GetAllRecordingsAsync(cancellationToken).ConfigureAwait(false);
            var result = new ChannelItemResult()
            {
                Items = new List<ChannelItemInfo>()
            };

            /// <summary>
            /// Optional Latest Folders
            /// </summary>
            /// 
            /// if (allRecordings != null)
            /// {
            ///     result.Items.Add(new ChannelItemInfo
            ///     {
            ///         Name = "Latest",
            ///         FolderType = ChannelFolderType.Container,
            ///         Id = "latest",
            ///         Type = ChannelItemType.Folder,
            ///         ImageUrl = ChannelFolderImage("Latest")
            ///     });
            /// }

            var series = allRecordings.FirstOrDefault(i => i.IsSeries);
            if (series != null)
            {
                result.Items.Add(new ChannelItemInfo
                {
                    Name = "TV Shows",
                    FolderType = ChannelFolderType.Container,
                    Id = "tvshows",
                    Type = ChannelItemType.Folder,
                    ImageUrl = series.ImageUrl
                });
            }

            var movies = allRecordings.FirstOrDefault(i => i.IsMovie);
            if (movies != null)
            {
                result.Items.Add(new ChannelItemInfo
                {
                    Name = "Movies",
                    FolderType = ChannelFolderType.Container,
                    Id = "movies",
                    Type = ChannelItemType.Folder,
                    ImageUrl = movies.ImageUrl
                });
            }

            var kids = allRecordings.FirstOrDefault(i => i.IsKids);
            if (kids != null)
            {
                result.Items.Add(new ChannelItemInfo
                {
                    Name = "Kids",
                    FolderType = ChannelFolderType.Container,
                    Id = "kids",
                    Type = ChannelItemType.Folder,
                    ImageUrl = kids.ImageUrl
                });
            }

            var news = allRecordings.FirstOrDefault(i => i.IsNews);
            if (news != null)
            {
                result.Items.Add(new ChannelItemInfo
                {
                    Name = "News & Documentary",
                    FolderType = ChannelFolderType.Container,
                    Id = "news",
                    Type = ChannelItemType.Folder,
                    ImageUrl = news.ImageUrl
                });
            }

            var sports = allRecordings.FirstOrDefault(i => i.IsSports);
            if (sports != null)
            {
                result.Items.Add(new ChannelItemInfo
                {
                    Name = "Sports",
                    FolderType = ChannelFolderType.Container,
                    Id = "sports",
                    Type = ChannelItemType.Folder,
                    ImageUrl = sports.ImageUrl
                });
            }

            var live = allRecordings.FirstOrDefault(i => i.IsLive);
            if (live != null)
            {
                result.Items.Add(new ChannelItemInfo
                {
                    Name = "Live Shows",
                    FolderType = ChannelFolderType.Container,
                    Id = "live",
                    Type = ChannelItemType.Folder,
                    ImageUrl = live.ImageUrl
                });
            }

            var other = allRecordings.FirstOrDefault(i => !i.IsSports && !i.IsNews && !i.IsMovie && !i.IsKids && !i.IsSeries);
            if (other != null && result.Items.Count > 0)
            {
                result.Items.Add(new ChannelItemInfo
                {
                    Name = "Other Shows",
                    FolderType = ChannelFolderType.Container,
                    Id = "others",
                    Type = ChannelItemType.Folder,
                    ImageUrl = other.ImageUrl,
                });
            }

            return result;
        }

        /// <summary> Gets recording name groups. </summary>
        /// <param name="query">             The query. </param>
        /// <param name="filter">            Specifies the filter. </param>
        /// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
        /// <returns> An asynchronous result that yields the recording name groups. </returns>
        private async Task<ChannelItemResult> GetRecordingNameGroups(InternalChannelItemQuery query, Func<MyRecordingInfo, bool> filter, CancellationToken cancellationToken)
        {
            var service = GetService();

            var allRecordings = await service.GetAllRecordingsAsync(cancellationToken).ConfigureAwait(false);
            var result = new ChannelItemResult()
            {
                Items = new List<ChannelItemInfo>(),
            };

            var doublenames = allRecordings.Where(filter)
                .GroupBy(i => i.Name).Where(i => i.Count() > 1).Select(i => i.Key)
                .ToLookup(i => i, StringComparer.OrdinalIgnoreCase);

            result.Items.AddRange(doublenames.OrderBy(i => i.Key).Select(i => new ChannelItemInfo
            {
                Name = i.Key,
                FolderType = ChannelFolderType.Container,
                Id = "name_" + i.Key.GetMD5().ToString("N"),
                Type = ChannelItemType.Folder,
                ImageUrl = string.Empty,
            }));

            var singlenames = allRecordings.Where(filter)
                .GroupBy(i => i.Name).Where(c => c.Count() == 1).Select(g => g.First());

            result.Items.AddRange(singlenames.Select(ConvertToChannelItem));

            result.Items.OrderBy(i => i.Name);

            return result;
        }

        /// <summary> Gets recording season groups. </summary>
        /// <param name="query">             The query. </param>
        /// <param name="filter">            Specifies the filter. </param>
        /// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
        /// <returns> An asynchronous result that yields the recording season groups. </returns>
        private async Task<ChannelItemResult> GetRecordingSeasonGroups(InternalChannelItemQuery query, Func<MyRecordingInfo, bool> filter, CancellationToken cancellationToken)
        {
            var service = GetService();

            var allRecordings = await service.GetAllRecordingsAsync(cancellationToken).ConfigureAwait(false);
            var result = new ChannelItemResult()
            {
                Items = new List<ChannelItemInfo>(),
            };

            var series = allRecordings.Where(filter).Select(i => i.Name).First();
            var seasons = allRecordings.Where(filter).GroupBy(i => i.SeasonNumber, i => i.Name, (key, g) => new { SeasonNumber = key, Name = g.ToList() });

            result.Items.AddRange(seasons.OrderBy(i => i.SeasonNumber).Select(i => new ChannelItemInfo
            {
                Name = "Season " + i.SeasonNumber,
                FolderType = ChannelFolderType.Season,
                Id = series + "_season_" + i.SeasonNumber.ToString().GetMD5().ToString("N"),
                Type = ChannelItemType.Folder,
                IndexNumber = i.SeasonNumber,
                SeriesName = series,
            }));

            return result;
        }

        /// <summary> Gets recording series groups. </summary>
        /// <param name="query">             The query. </param>
        /// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
        /// <returns> An asynchronous result that yields the recording series groups. </returns>
        private async Task<ChannelItemResult> GetRecordingSeriesGroups(InternalChannelItemQuery query, CancellationToken cancellationToken)
        {
            var service = GetService();

            var allRecordings = await service.GetAllRecordingsAsync(cancellationToken).ConfigureAwait(false);
            var result = new ChannelItemResult()
            {
                Items = new List<ChannelItemInfo>(),
            };

            var series = allRecordings
                .Where(i => i.IsSeries)
                .ToLookup(i => i.Name, StringComparer.OrdinalIgnoreCase);

            result.Items.AddRange(series.OrderBy(i => i.Key).Select(i => new ChannelItemInfo
            {
                Name = i.Key,
                SeriesName = i.Key,
                FolderType = ChannelFolderType.Container,
                //FolderType = ChannelFolderType.Series,
                Id = "series_" + i.Key.GetMD5().ToString("N"),
                Type = ChannelItemType.Folder,
                ImageUrl = String.Empty,
            }));

            return result;
        }

        /// <summary> Gets the service. </summary>
        /// <returns> The service. </returns>
        private LiveTvServiceBase GetService()
        {
            return _liveTvManager.Services.OfType<LiveTvServiceBase>().First();
        }

        /// <summary> Updates the user interface for the timer callback action. </summary>
        /// <param name="state"> The state. </param>
        private void OnUpdateTimerCallback(object state)
        {
            OnContentChanged();
        }
        #endregion
    }

    /// <summary> Information about my recording. </summary>
    public class MyRecordingInfo
    {
        #region [Constructors]
        /// <summary> Initializes a new instance of the Emby.Plugins.DVBLogic.MyRecordingInfo class. </summary>
        public MyRecordingInfo()
        {
            Genres = new List<string>();
        }
        #endregion

        #region [Public properties]
        /// <summary> ChannelId of the recording. </summary>
        /// <value> The identifier of the channel. </value>
        public string ChannelId { get; set; }

        /// <summary> Gets or sets the channel logo image. </summary>
        /// <value> The channel logo image. </value>
        public string ChannelLogo { get; set; }

        /// <summary> Gets or sets the type of the channel name. </summary>
        /// <value> The channel name. </value>
        public string ChannelName { get; set; }

        /// <summary> Gets or sets the type of the channel. </summary>
        /// <value> The type of the channel. </value>
        public ChannelType ChannelType { get; set; }

        /// <summary> Gets or sets the community rating. </summary>
        /// <value> The community rating. </value>
        public float? CommunityRating { get; set; }

        /// <summary> Gets or sets the date last updated. </summary>
        /// <value> The date last updated. </value>
        public DateTimeOffset DateLastUpdated { get; set; }

        /// <summary> Gets or sets the EIT content. </summary>
        /// <value> The EIT content. </value>
        public string EitContent { get; set; }

        /// <summary> The end date of the recording, in UTC. </summary>
        /// <value> The end date. </value>
        public DateTimeOffset EndDate { get; set; }

        /// <summary> Gets or sets the episode number. </summary>
        /// <value> The episode number. </value>
        public int? EpisodeNumber { get; set; }

        /// <summary> Gets or sets the episode title. </summary>
        /// <value> The episode title. </value>
        public string EpisodeTitle { get; set; }

        /// <summary> Genre of the program. </summary>
        /// <value> The genres. </value>
        public List<string> Genres { get; set; }

        /// <summary> Gets or sets a value indicating whether this instance has image. </summary>
        /// <value> <c>null</c> if [has image] contains no value, <c>true</c> if [has image]; otherwise, <c>false</c>. </value>
        public bool? HasImage { get; set; }

        /// <summary> Id of the recording. </summary>
        /// <value> The identifier. </value>
        public string Id { get; set; }

        /// <summary> Supply the image path if it can be accessed directly from the file system. </summary>
        /// <value> The image path. </value>
        public string ImagePath { get; set; }

        /// <summary> Supply the image url if it can be downloaded. </summary>
        /// <value> The image URL. </value>
        public string ImageUrl { get; set; }

        /// <summary> Gets or sets a value indicating whether this instance is hd. </summary>
        /// <value> <c>true</c> if this instance is hd; otherwise, <c>false</c>. </value>
        public bool? IsHD { get; set; }

        /// <summary> Gets or sets a value indicating whether this instance is kids. </summary>
        /// <value> <c>true</c> if this instance is kids; otherwise, <c>false</c>. </value>
        public bool IsKids { get; set; }

        /// <summary> Gets or sets a value indicating whether this instance is live. </summary>
        /// <value> <c>true</c> if this instance is live; otherwise, <c>false</c>. </value>
        public bool IsLive { get; set; }

        /// <summary> Gets or sets a value indicating whether this instance is movie. </summary>
        /// <value> <c>true</c> if this instance is movie; otherwise, <c>false</c>. </value>
        public bool IsMovie { get; set; }

        /// <summary> Gets or sets a value indicating whether this instance is news. </summary>
        /// <value> <c>true</c> if this instance is news; otherwise, <c>false</c>. </value>
        public bool IsNews { get; set; }

        /// <summary> Gets or sets a value indicating whether this instance is premiere. </summary>
        /// <value> <c>true</c> if this instance is premiere; otherwise, <c>false</c>. </value>
        public bool IsPremiere { get; set; }

        /// <summary> Gets or sets a value indicating whether this instance is repeat. </summary>
        /// <value> <c>true</c> if this instance is repeat; otherwise, <c>false</c>. </value>
        public bool IsRepeat { get; set; }

        /// <summary> Gets or sets a value indicating whether this instance is series. </summary>
        /// <value> <c>true</c> if this instance is series; otherwise, <c>false</c>. </value>
        public bool IsSeries { get; set; }

        /// <summary> Gets or sets a value indicating whether this instance is sports. </summary>
        /// <value> <c>true</c> if this instance is sports; otherwise, <c>false</c>. </value>
        public bool IsSports { get; set; }

        /// <summary> Gets or sets information describing the media source. </summary>
        /// <value> Information describing the media source. </value>
        public MediaSourceInfo MediaSourceInfo { get; set; }

        /// <summary> Name of the recording. </summary>
        /// <value> The name. </value>
        public string Name { get; set; }

        /// <summary> Gets or sets the official rating. </summary>
        /// <value> The official rating. </value>
        public string OfficialRating { get; set; }

        /// <summary> Gets or sets the original air date. </summary>
        /// <value> The original air date. </value>
        public DateTimeOffset? OriginalAirDate { get; set; }

        /// <summary> Gets or sets the overview. </summary>
        /// <value> The overview. </value>
        public string Overview { get; set; }

        /// <summary> Gets or sets the path. </summary>
        /// <value> The path. </value>
        public string Path { get; set; }

        /// <summary> Gets or sets the program identifier. </summary>
        /// <value> The program identifier. </value>
        public string ProgramId { get; set; }

        /// <summary> Gets or sets the season number. </summary>
        /// <value> The season number. </value>
        public int? SeasonNumber { get; set; }

        /// <summary> Gets or sets the series timer identifier. </summary>
        /// <value> The series timer identifier. </value>
        public string SeriesTimerId { get; set; }

        /// <summary> Gets or sets the show identifier. </summary>
        /// <value> The show identifier. </value>
        public string ShowId { get; set; }

        /// <summary> The start date of the recording, in UTC. </summary>
        /// <value> The start date. </value>
        public DateTimeOffset StartDate { get; set; }

        /// <summary> Gets or sets the status. </summary>
        /// <value> The status. </value>
        public RecordingStatus Status { get; set; }

        /// <summary> Gets or sets the timer identifier. </summary>
        /// <value> The timer identifier. </value>
        public string TimerId { get; set; }

        /// <summary> Supply the poster downloaded from TMDB. </summary>
        /// <value> The image path. </value>
        public string TmdbPoster { get; set; }

        /// <summary> Gets or sets the URL. </summary>
        /// <value> The URL. </value>
        public string Url { get; set; }

        /// <summary> Gets or sets the year of the title. </summary>
        /// <value> The episode number. </value>
        public int? Year { get; set; }
        #endregion
    }
}