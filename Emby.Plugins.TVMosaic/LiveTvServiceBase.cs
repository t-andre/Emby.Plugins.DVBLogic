// <copyright file="LiveTvServiceBase.cs" >
// Copyright (c) 2017 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>01.09.2017</date>
// <summary>Implements the live TV service class</summary>
using System;
using System.Linq;
using System.Threading;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediaBrowser.Model.Dto;
using MediaBrowser.Controller.LiveTv;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Session;
using MediaBrowser.Controller.Drawing;

namespace Emby.Plugins.DVBLogic
{
	/// <summary> A live TV service base. </summary>
	/// <seealso cref="T:MediaBrowser.Controller.LiveTv.ILiveTvService"/>
	public abstract class LiveTvServiceBase 
	{
		#region [Constructors]

		/// <summary> Initializes a new instance of the Emby.Plugins.DVBLink.LiveTvService class. </summary>
		/// <param name="plugin">				  The plugin. </param>
		/// <param name="serverApplicationPaths"> The server application paths. </param>
		/// <param name="sessionManager">		  Manager for session. </param>
		public LiveTvServiceBase(IDVBLogicPlugin plugin, IServerApplicationPaths serverApplicationPaths, ISessionManager sessionManager)
		{
			this.DVBLogicPlugin = plugin;
			this._ServerApplicationPaths = serverApplicationPaths;
			this._SessionManager = sessionManager;
			this._SessionManager.PlaybackStart += this.SessionManagerPlaybackStart;
			this._SessionManager.PlaybackStopped += this.SessionManagerPlaybackStopped;
		}

		/// <summary> Event handler. Called by _SessionManager for playback stopped events. </summary>
		/// <param name="sender"> Source of the event. </param>
		/// <param name="e"> Playback stop event information. </param>
		private void SessionManagerPlaybackStopped(object sender, MediaBrowser.Controller.Library.PlaybackStopEventArgs e)
		{
			var session = this.DVBLogicPlugin.TVProxy.Client.SessionManager.GetSession(e.MediaSourceId);
			if (e != null && session != null)
			{
				this.DVBLogicPlugin.TVProxy.Client.SessionManager.RemoveSession(e.MediaSourceId);
			}
		}

		/// <summary> Event handler. Called by _SessionManager for playback start events. </summary>
		/// <param name="sender"> Source of the event. </param>
		/// <param name="e"> Playback progress event information. </param>
		private void SessionManagerPlaybackStart(object sender, MediaBrowser.Controller.Library.PlaybackProgressEventArgs e)
		{
			var session = this.DVBLogicPlugin.TVProxy.Client.SessionManager.GetSession(e.MediaSourceId);
			if (e != null && session != null)
			{
				session.DeviceId = e.DeviceId;
				session.DeviceName = e.DeviceName;
				session.ClientName = e.ClientName;
				if (e.Users?.Count > 0)
				{
					session.UserName = e.Users.Select(x => x.Name).Aggregate((current, next) => current + ", " + next);
				}
			}
		}

		#endregion

		#region [Private fields]

		/// <summary> Manager for session. </summary>
		private readonly ISessionManager _SessionManager;

		/// <summary> The server application paths. </summary>
		private readonly IServerApplicationPaths _ServerApplicationPaths;

		/// <summary> Gets the plugin. </summary>
		/// <value> The plugin. </value>
		protected IDVBLogicPlugin DVBLogicPlugin { get; }

		#endregion

		#region [Public properties]
		/// <summary> Gets URL of the home page. </summary>
		/// <value> The home page URL. </value>
		public abstract string HomePageUrl { get; }

		/// <summary> Gets the name. </summary>
		/// <value> The name. </value>
		public abstract string Name { get; }

		public DateTimeOffset LastRecordingChange = DateTimeOffset.MinValue;
		#endregion

		#region [Events]

		/// <summary> Occurs when [data source changed]. </summary>
		public event EventHandler DataSourceChanged;

		/// <summary> Occurs when [recording status changed]. </summary>
		public event EventHandler<RecordingStatusChangedEventArgs> RecordingStatusChanged;

		#endregion

		#region [Server]
		/// <summary> Gets status information asynchronous. </summary>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result that yields the status information asynchronous. </returns>
		public Task<LiveTvServiceStatusInfo> GetStatusInfoAsync(CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.GetStatusInfoAsync(cancellationToken);
		}

		/// <summary> Resets the tuner. </summary>
		/// <param name="id"> The identifier. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result. </returns>
		public Task ResetTuner(string id, CancellationToken cancellationToken)
		{
			return Task.Delay(0, cancellationToken);
		}

		#endregion

		#region [Channels]
		/// <summary> Gets channels asynchronous. </summary>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns>
		/// The asynchronous result that yields an enumerator that allows foreach to be used to process
		/// the channels asynchronous in this collection. </returns>
		public Task<IEnumerable<ChannelInfo>> GetChannelsAsync(CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.GetChannelsAsync(cancellationToken, this._ServerApplicationPaths.InternalMetadataPath);
		}

		/// <summary> Gets channel image asynchronous. </summary>
		/// <param name="channelId"> Identifier for the channel. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result that yields the channel image asynchronous. </returns>
		public Task<ImageStream> GetChannelImageAsync(string channelId, CancellationToken cancellationToken)
		{
			return Task.FromResult<ImageStream>(null);
		}

		#endregion

		#region [Guide]
		/// <summary> Gets program image asynchronous. </summary>
		/// <param name="programId"> Identifier for the program. </param>
		/// <param name="channelId"> Identifier for the channel. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result that yields the program image asynchronous. </returns>
		public Task<MediaBrowser.Controller.Drawing.ImageStream> GetProgramImageAsync(string programId, string channelId, CancellationToken cancellationToken)
		{
			return Task.FromResult<ImageStream>(null);
		}

		/// <summary> Gets programs asynchronous. </summary>
		/// <param name="channelId"> Identifier for the channel. </param>
		/// <param name="startDateUtc"> The start date UTC. </param>
		/// <param name="endDateUtc"> The end date UTC. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns>
		/// The asynchronous result that yields an enumerator that allows foreach to be used to process
		/// the programs asynchronous in this collection. </returns>
		public Task<IEnumerable<ProgramInfo>> GetProgramsAsync(string channelId, DateTimeOffset startDateUtc, DateTimeOffset endDateUtc, CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.GetProgramsAsync(channelId, startDateUtc, endDateUtc, cancellationToken);
		}
		#endregion

		#region [Recordings]
		/// <summary> Gets recording image asynchronous. </summary>
		/// <param name="recordingId"> Identifier for the recording. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result that yields the recording image asynchronous. </returns>
		public Task<ImageStream> GetRecordingImageAsync(string recordingId, CancellationToken cancellationToken)
		{
			return Task.FromResult<ImageStream>(null);
		}

		/// <summary> Gets recordings asynchronous. </summary>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> An asynchronous result that yields the recordings. </returns>
		public Task<IEnumerable<RecordingInfo>> GetRecordingsAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult<IEnumerable<RecordingInfo>>(new List<RecordingInfo>());
		}

		/// <summary> Gets the All Recordings async. </summary>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> Task{IEnumerable{RecordingInfo}} </returns>
		public Task<IEnumerable<MyRecordingInfo>> GetAllRecordingsAsync(CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.GetAllRecordingsAsync(cancellationToken);
		}

		/// <summary> Gets recording stream. </summary>
		/// <param name="recordingId"> Identifier for the recording. </param>
		/// <param name="mediaSourceId"> Identifier for the media source. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result that yields the recording stream. </returns>
		public Task<MediaSourceInfo> GetRecordingStream(string recordingId, string mediaSourceId, CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.GetRecordingStreamAsync(recordingId, mediaSourceId, cancellationToken);
		}

		/// <summary> Gets recording stream media sources. </summary>
		/// <param name="recordingId"> Identifier for the recording. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result that yields the recording stream media sources. </returns>
		public Task<List<MediaSourceInfo>> GetRecordingStreamMediaSources(string recordingId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <summary> Deletes the recording asynchronous. </summary>
		/// <param name="recordingId"> Identifier for the recording. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result. </returns>
		public Task DeleteRecordingAsync(string recordingId, CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.DeleteRecordingAsync(recordingId, cancellationToken);
		}

		#endregion

		#region [Schedule]
		/// <summary> Gets timers asynchronous. </summary>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns>
		/// The asynchronous result that yields an enumerator that allows foreach to be used to process
		/// the timers asynchronous in this collection. </returns>
		public Task<IEnumerable<TimerInfo>> GetTimersAsync(CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.GetTimersAsync(cancellationToken);
		}

		/// <summary> Creates timer asynchronous. </summary>
		/// <param name="info"> The information. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result. </returns>
		public Task CreateTimerAsync(TimerInfo info, CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.CreateTimerAsync(info, cancellationToken);
		}

		/// <summary> Updates the timer asynchronous. </summary>
		/// <param name="info"> The information. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result. </returns>
		public Task UpdateTimerAsync(TimerInfo info, CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.UpdateTimerAsync(info, cancellationToken);
		}

		/// <summary> Cancel timer asynchronous. </summary>
		/// <param name="timerId"> Identifier for the timer. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result. </returns>
		public Task CancelTimerAsync(string timerId, CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.CancelTimerAsync(timerId, cancellationToken);
		}

		/// <summary> Gets new timer defaults asynchronous. </summary>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <param name="program"> (Optional) The program. </param>
		/// <returns> The asynchronous result that yields the new timer defaults asynchronous. </returns>
		public Task<SeriesTimerInfo> GetNewTimerDefaultsAsync(CancellationToken cancellationToken, ProgramInfo program = null)
		{
			return this.DVBLogicPlugin.TVProxy.GetNewTimerDefaultsAsync(cancellationToken);
		}

		#endregion

		#region [Series]
		/// <summary> Gets series timers asynchronous. </summary>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns>
		/// The asynchronous result that yields an enumerator that allows foreach to be used to process
		/// the series timers asynchronous in this collection. </returns>
		public Task<IEnumerable<SeriesTimerInfo>> GetSeriesTimersAsync(CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.GetSeriesTimersAsync(cancellationToken);
		}

		/// <summary> Creates series timer asynchronous. </summary>
		/// <param name="info"> The information. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result. </returns>
		public Task CreateSeriesTimerAsync(SeriesTimerInfo info, CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.CreateSeriesTimerAsync(info, cancellationToken);
		}

		/// <summary> Updates the series timer asynchronous. </summary>
		/// <param name="info"> The information. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result. </returns>
		public Task UpdateSeriesTimerAsync(SeriesTimerInfo info, CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.UpdateSeriesTimerAsync(info, cancellationToken);
		}

		/// <summary> Cancel series timer asynchronous. </summary>
		/// <param name="timerId"> Identifier for the timer. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result. </returns>
		public Task CancelSeriesTimerAsync(string timerId, CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.CancelSeriesTimerAsync(timerId, cancellationToken);
		}
		#endregion

		#region [LiveTV]
		/// <summary> Gets channel stream. </summary>
		/// <param name="channelId"> Identifier for the channel. </param>
		/// <param name="mediaSourceId"> Identifier for the media source. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result that yields the channel stream. </returns>
		public Task<MediaSourceInfo> GetChannelStream(string channelId, string mediaSourceId, CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.GetChannelStreamAsync(channelId, mediaSourceId, cancellationToken);
		}

		/// <summary> Gets channel stream media sources. </summary>
		/// <param name="channelId"> Identifier for the channel. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result that yields the channel stream media sources. </returns>
		public Task<List<MediaSourceInfo>> GetChannelStreamMediaSources(string channelId, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
			//return await Task.FromResult(Plugin.TVProxy.GetChannelStreamMediaSources(channelId));
		}

		/// <summary> Record live stream. </summary>
		/// <param name="id"> The identifier. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result. </returns>
		public Task RecordLiveStream(string id, CancellationToken cancellationToken)
		{
			return Task.Delay(0, cancellationToken);
		}

		/// <summary> Closes live stream. </summary>
		/// <param name="id"> The identifier. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result. </returns>
		public Task CloseLiveStream(string id, CancellationToken cancellationToken)
		{
			return this.DVBLogicPlugin.TVProxy.CloseLiveStreamAsync(id, cancellationToken);
		}

		#endregion

	}
}
