// <copyright file="TVServerClient.cs" >
// Copyright (c) 2018 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares</author>
// <date>27.08.2018</date>
// <summary>Implements the TV server client class</summary>
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TSoft.TVServer.Configurations;
using TSoft.TVServer.Constants;
using TSoft.TVServer.Entities;
using TSoft.TVServer.Helpers;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer
{
	/// <summary> A TV server client. </summary>
	/// <seealso cref="T:TSoft.TVServer.ITVServerClient"/>
	public class TVServerClient : ITVServerClient
	{
		#region [Constructors]

		/// <summary> Initializes a new instance of the TSoft.TVServer.TVServerClient class. </summary>
		/// <param name="clientType"> The type of the client. </param>
		public TVServerClient(EnumTVServerClientType clientType)
		{
			this._Logger = new NullLogger();
			this.ClientType = clientType;
			this.Name = this.ClientType == EnumTVServerClientType.TVMosaic ? "TVMosaic" : "DVBLink";
			//this.HttpClient = new DVBLinkHttpClient(this.Logger);
			//this.InitializeHttpClient();
		}

		#endregion

		#region [Readonly fields]

		/// <summary> The lazy. </summary>
		private static readonly Lazy<TVServerClient> lazy = new Lazy<TVServerClient>(() => new TVServerClient(EnumTVServerClientType.TVMosaic));

		#endregion

		#region [Private Fields]

		/// <summary> Manager for session. </summary>
		private SessionManager _SessionManager;

		/// <summary> The configuration. </summary>
		private ServerConfiguration _Config;

		/// <summary> The logger. </summary>
		private AbstractLogger _Logger;

		/// <summary> The connection error retry. </summary>
		private const int _ConnectionErrorRetry = 4;

		/// <summary> The status. </summary>
		private ServerStatus _Status;

		/// <summary> Information describing the tuner. </summary>
		private TunerInfo _TunerInfo;

		#endregion

		#region [Public Properties]

		/// <summary> Gets the configuration. </summary>
		/// <value> The configuration. </value>
		/// <seealso cref="P:TSoft.TVServer.ITVServerClient.Config"/>
		public ServerConfiguration Config
		{
			get
			{
				if (this._Config == null)
				{
					int serverPort = this.ClientType == EnumTVServerClientType.TVMosaic ? 9270 : 8100;
					int serverStreamPort = this.ClientType == EnumTVServerClientType.TVMosaic ? 9271 : 8101;
					this._Config = new ServerConfiguration("localhost", serverPort, serverStreamPort);
				}
				return this._Config;
			}
			private set { this._Config = value; }
		}

		/// <summary> Gets the HTTP client. </summary>
		/// <value> The HTTP client. </value>
		/// <seealso cref="P:TSoft.TVServer.ITVServerClient.HttpClient"/>
		public HttpClientBase HttpClient { get; private set; }

		/// <summary> Gets the instance. </summary>
		/// <value> The instance. </value>
		public static TVServerClient Instance
		{
			get { return lazy.Value; }
		}

		/// <summary> Gets the identifier of the client. </summary>
		/// <value> The identifier of the client. </value>
		/// <seealso cref="P:TSoft.TVServer.ITVServerClient.ClientId"/>
		public string ClientId
		{
			get { return this.ClientType == EnumTVServerClientType.TVMosaic ? "BD4C3582-AA2E-4C89-B816-F0EEF937CAEE" : "61A0D104-2FC1-473A-8954-CD6AAB1BE0D9"; }
		}

		/// <summary> Gets a value indicating whether this TVServerClient is enabled. </summary>
		/// <value> True if enable, false if not. </value>
		public bool Enable
		{
			get
			{
				return (this.Config?.Enable ?? false);
			}
		}

		/// <summary> The logger. </summary>
		/// <value> The logger. </value>
		/// <seealso cref="P:TSoft.TVServer.ITVServerClient.Logger"/>
		public AbstractLogger Logger
		{
			get { return this._Logger; }
		}

		/// <summary> Gets the status. </summary>
		/// <value> The status. </value>
		/// <seealso cref="P:TSoft.TVServer.ITVServerClient.Status"/>
		public ServerStatus Status
		{
			get
			{
				return this._Status ?? (this._Status = new ServerStatus());
			}
			private set { this._Status = value; }
		}

		/// <summary> Gets the manager for session. </summary>
		/// <value> The session manager. </value>
		/// <seealso cref="P:TSoft.TVServer.ITVServerClient.SessionManager"/>
		public SessionManager SessionManager
		{
			get
			{
				return this._SessionManager ?? (this._SessionManager = new SessionManager());
			}
			private set { this._SessionManager = value; }
		}

		/// <summary> Gets or sets information describing the tuner. </summary>
		/// <value> Information describing the tuner. </value>
		/// <seealso cref="P:TSoft.TVServer.ITVServerClient.TunerInfo"/>
		public TunerInfo TunerInfo
		{
			get
			{
				return this._TunerInfo ?? (this._TunerInfo = new TunerInfo());
			}
			set { this._TunerInfo = value; }
		}

		/// <summary> Gets or sets a value indicating whether the debug log. </summary>
		/// <value> true if debug log, false if not. </value>
		/// <seealso cref="P:TSoft.TVServer.ITVServerClient.DebugLog"/>
		public bool DebugLog { get; set; }

		/// <summary> Gets the type of the client. </summary>
		/// <value> The type of the client. </value>
		public EnumTVServerClientType ClientType { get; private set; }

		/// <summary> Gets the name. </summary>
		/// <value> The name. </value>
		public string Name { get; private set; }
		#endregion

		#region [Public Methods]

		/// <summary> Initializes this TSoft.TVServer.DVBLinkClient. </summary>
		/// <param name="logger">	  The logger. </param>
		/// <param name="config">	  (Optional) The configuration. </param>
		/// <param name="httpClient"> (Optional) The HTTP client. </param>
		/// <returns> true if it succeeds, false if it fails. </returns>
		/// <seealso cref="M:TSoft.TVServer.ITVServerClient.Initialize(AbstractLogger,ServerConfiguration,HttpClientBase)"/>
		public bool Initialize(AbstractLogger logger, ServerConfiguration config = null, HttpClientBase httpClient = null)
		{
			this.Status.Reinitialize();
			this.UpdateLogger(logger);
			this.Logger.Info("Initializing");
			if (config != null)
				this.UpdateConfig(config);
			if (httpClient != null)
				this.UpdateClient(httpClient);
			Task.Run(async () => await this.EnsureConnectionAsync(CancellationToken.None).ConfigureAwait(false)).Wait(TimeSpan.FromSeconds(15));
			return true;
		}

		/// <summary> Initializes this TVServerClient. </summary>
		/// <returns> True if it succeeds, false if it fails. </returns>
		public bool Initialize()
		{
			this.Status.Reinitialize();
			Task.Run(async () => await this.EnsureConnectionAsync(CancellationToken.None).ConfigureAwait(false)).Wait(TimeSpan.FromSeconds(15));
			return true;
		}

		/// <summary> Connects to server asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <param name="reconnect">		 (Optional) True to reconnect. </param>
		/// <returns> An asynchronous result. </returns>
		public Task ConnectToServerAsync(CancellationToken cancellationToken, bool reconnect = false)
		{
			return this.EnsureConnectionAsync(cancellationToken);
		}

		/// <summary> Updates the configuration described by config. </summary>
		/// <param name="config"> The configuration. </param>
		/// <returns> An IDVBLinkClient. </returns>
		/// <seealso cref="M:TSoft.TVServer.ITVServerClient.UpdateConfig(ServerConfiguration)"/>
		public ITVServerClient UpdateConfig(ServerConfiguration config)
		{
			this.Config = config;
			this.Logger.Info("Configuration updated");
			this.Status.Reinitialize();
			if (this.HttpClient != null)
			{
				this.InitializeHttpClient();
				Task.Run(async () => await this.EnsureConnectionAsync(CancellationToken.None).ConfigureAwait(false)).Wait(TimeSpan.FromSeconds(15));
			}
			return this;
		}

		/// <summary> Updates the logger described by logger. </summary>
		/// <param name="logger"> The logger. </param>
		/// <returns> An IDVBLinkClient. </returns>
		/// <seealso cref="M:TSoft.TVServer.ITVServerClient.UpdateLogger(AbstractLogger)"/>
		public ITVServerClient UpdateLogger(AbstractLogger logger)
		{
			this._Logger = logger;

			return this;
		}

		/// <summary> Updates the client described by httpClient. </summary>
		/// <param name="httpClient"> The HTTP client. </param>
		/// <returns> An ITVServerClient. </returns>
		public ITVServerClient UpdateClient(HttpClientBase httpClient)
		{
			this.HttpClient = httpClient;
			this.InitializeHttpClient();

			return this;
		}

		/// <summary> Initializes the HTTP client. </summary>
		public void InitializeHttpClient()
		{
			this.HttpClient.UserName = this.Config.UserName;
			this.HttpClient.Password = this.Config.Password;
			this.HttpClient.Url = this.Config.ApiUrl;
			this.HttpClient.RequiresAuthentication = this.Config.UseAuthorization;
			this.HttpClient.ClientInitialized = false;
		}
		#endregion

		#region [Response]

		/// <summary> Gets response object asynchronous. </summary>
		/// <typeparam name="T"> Generic type parameter. </typeparam>
		/// <typeparam name="R"> Type of the r. </typeparam>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <param name="message">			 (Optional) The message. </param>
		/// <param name="checkResultObject"> (Optional) True to check result object. </param>
		/// <returns> An asynchronous result that yields the response object async&lt; t,r&gt; </returns>
		public async Task<T> GetResponseObjectAsync<T, R>(R request, CancellationToken cancellationToken, string message = "", bool checkResultObject = true)
			where T : class
			where R : class, IRequest
		{
			try
			{
				await this.EnsureConnectionAsync(cancellationToken).ConfigureAwait(false);
				if (this.Status.IsAvailable)
				{
					if (this.DebugLog)
						this.Logger.Info("Command : {0}", message);
					ResponseObject<T, R> obj = await this.HttpClient.GetResponseObjectAsync<T, R>(request, cancellationToken).ConfigureAwait(false);
					this.CheckResponseObject(obj, checkResultObject);
					return obj.ResultObject;
				}
			}
			catch (Exception ex)
			{
				this.Logger.ErrorException("{0} failed : ", ex, message);
			}

			return null;
		}

		/// <summary> Gets response asynchronous. </summary>
		/// <typeparam name="R"> Type of the r. </typeparam>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <param name="message">			 (Optional) The message. </param>
		/// <param name="checkResultObject"> (Optional) True to check result object. </param>
		/// <returns> An asynchronous result that yields the response async&lt; r&gt; </returns>
		public async Task<Response> GetResponseAsync<R>(R request, CancellationToken cancellationToken, string message = "", bool checkResultObject = true)
			where R : class, IRequest
		{
			try
			{
				await this.EnsureConnectionAsync(cancellationToken).ConfigureAwait(false);
				if (this.Status.IsAvailable)
				{
					if (this.DebugLog)
						this.Logger.Info("Command : {0}", message);
					var obj = await this.HttpClient.GetResponseAsync<R>(request, cancellationToken).ConfigureAwait(false);
					this.CheckResponse(obj, checkResultObject);
					return obj;
				}
			}
			catch (Exception ex)
			{
				this.Logger.ErrorException("{0} failed : ", ex, message);
			}

			return null;
		}

		/// <summary> Check response object. </summary>
		/// <exception cref="NullReferenceException">    Thrown when a value was unexpectedly null. </exception>
		/// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
		/// <typeparam name="T"> Generic type parameter. </typeparam>
		/// <param name="result">			 The result. </param>
		/// <param name="checkResultObject"> (Optional) True to check result object. </param>
		private void CheckResponseObject<T>(ResponseObject<T> result, bool checkResultObject = true) where T : class
		{
			if (result == null)
				throw new NullReferenceException(string.Format("result is null"));

			if (result.Status != EnumStatusCode.STATUS_OK)
				throw new InvalidOperationException(this.FormatDVBLinkStatusError(result.Status), result.Exception);

			if (checkResultObject == true && result.ResultObject == null)
				throw new NullReferenceException(string.Format("result object is null"));
		}

		/// <summary> Check response. </summary>
		/// <exception cref="NullReferenceException">    Thrown when a value was unexpectedly null. </exception>
		/// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
		/// <param name="result">			 The result. </param>
		/// <param name="checkResultObject"> (Optional) True to check result object. </param>
		private void CheckResponse(Response result, bool checkResultObject = true)
		{
			if (result == null)
				throw new NullReferenceException(string.Format("result is null"));

			if (result.Status != EnumStatusCode.STATUS_OK)
				throw new InvalidOperationException(this.FormatDVBLinkStatusError(result.Status), result.Exception);
		}

		/// <summary> Format dvb link status error. </summary>
		/// <param name="statusCode"> The status code. </param>
		/// <returns> The formatted dvb link status error. </returns>
		private string FormatDVBLinkStatusError(EnumStatusCode statusCode)
		{
			return string.Format("Server Status : {0}", statusCode.ToString());
		}

		#endregion

		#region [StreamRequest]

		/// <summary> Gets indirect channel stream. </summary>
		/// <param name="streamSourceInfo"> Information describing the stream source. </param>
		/// <param name="clientId">		    Identifier for the client. </param>
		/// <param name="channelId">	    Identifier for the channel. </param>
		/// <param name="defaultLanguage">  (Optional) The default language. </param>
		/// <param name="duration">		    (Optional) The duration. </param>
		/// <returns> The indirect channel stream. </returns>
		public PlayChannelRequest GetIndirectChannelStream(StreamSourceInfo streamSourceInfo, string clientId, string channelId, string defaultLanguage = "", long duration = 0)
		{
			var request = new PlayChannelRequest(this.Config.ServerIp, channelId, clientId, streamSourceInfo.StreamType);
			request.StreamType = streamSourceInfo.StreamType;
			if (duration > 0)
				request.Duration = duration * 60;

			if (streamSourceInfo.Transcode)
			{
				request.Transcoder = new Transcoder();
				if (streamSourceInfo.Transcoder.Height != null)
					request.Transcoder.Height = streamSourceInfo.Transcoder.Height;
				if (streamSourceInfo.Transcoder.Width != null)
					request.Transcoder.Width = streamSourceInfo.Transcoder.Width;
				if (streamSourceInfo.Transcoder.Bitrate != null)
					request.Transcoder.Bitrate = GlobalExtensions.ConvertToKiloBits(streamSourceInfo.Transcoder.Bitrate);
				if (!string.IsNullOrEmpty(streamSourceInfo.Transcoder.Language))
					request.Transcoder.AudioTrack = streamSourceInfo.Transcoder.Language;
				else if (!string.IsNullOrEmpty(defaultLanguage))
					request.Transcoder.AudioTrack = defaultLanguage;
			}

			return request;
		}

		/// <summary> Gets direct channel stream. </summary>
		/// <param name="clientId">  Identifier for the client. </param>
		/// <param name="channelId"> Identifier for the channel. </param>
		/// <returns> The direct channel stream. </returns>
		public string GetDirectChannelStream(string clientId, string channelId)
		{
            string urlbind = this.Status.Version > 6 ? "stream" : "dvblink";
            string url = $"http://{Config.ServerIp}:{Config.StreamingPort}/{urlbind}/direct?";
            url += $"client={clientId}";
			url += $"&channel={channelId}";
			return url;
		}

		/// <summary> Gets direct channel stream. </summary>
		/// <param name="streamSourceInfo"> Information describing the stream source. </param>
		/// <param name="clientId">		    Identifier for the client. </param>
		/// <param name="channelId">	    Identifier for the channel. </param>
		/// <param name="defaultLanguage">  (Optional) The default language. </param>
		/// <returns> The direct channel stream. </returns>
		public string GetDirectChannelStream(StreamSourceInfo streamSourceInfo, string clientId, string channelId, string defaultLanguage = "")
		{
			string url = this.GetDirectChannelStream(clientId, channelId);
			//url += string.Format("&transcoder={0}", GlobalExtensions.GetStreamType(streamSourceInfo.StreamType));
			if (streamSourceInfo.Transcode & 1 == 2)
			{
				if (streamSourceInfo.Transcoder.Height != null)
					url += string.Format("&height={0}", streamSourceInfo.Transcoder.Height);
				if (streamSourceInfo.Transcoder.Width != null)
					url += string.Format("&width={0}", streamSourceInfo.Transcoder.Width);
				if (streamSourceInfo.Transcoder.Bitrate != null)
					url += string.Format("&bitrate={0}", GlobalExtensions.ConvertToKiloBits(streamSourceInfo.Transcoder.Bitrate));
				if (!string.IsNullOrEmpty(streamSourceInfo.Transcoder.Language))
					url += string.Format("&lng={0}", streamSourceInfo.Transcoder.Language);
				else if (!string.IsNullOrEmpty(defaultLanguage))
					url += string.Format("&lng={0}", defaultLanguage);
			}

			return url;
		}
		#endregion

		#region [Channels]

		/// <summary> Gets channels asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the channels. </returns>
		public Task<Channels> GetChannelsAsync(CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<Channels, ChannelsRequest>(new ChannelsRequest(), cancellationToken, "Get Channels");
		}

		/// <summary> Gets channels URL asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the channels URL. </returns>
		public Task<ChannelUrl> GetChannelsUrlAsync(ChannelUrlRequest request, CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<ChannelUrl, ChannelUrlRequest>(request, cancellationToken, "Get Channels Url");
		}

		/// <summary> Updates the channels. </summary>
		/// <param name="channels"> The channels. </param>
		/// <param name="logoPath"> Full pathname of the logo file. </param>
		/// <returns> The Channels. </returns>
		public Channels UpdateChannels(Channels channels, string logoPath)
		{
			var haLogosDirectory = true;
			string logosDirectory = string.Empty;
			if (this.Config.UseServerLogos)
			{
				this.Logger.Info("Get Logos from server");
			}
			else
			{
				logosDirectory = this.Config.LogosPath;
				if (!Directory.Exists(logosDirectory))
				{
					logosDirectory = Path.Combine(logoPath, "LiveTvLogos");
					if (!Directory.Exists(logosDirectory))
						haLogosDirectory = false;
				}

				if (haLogosDirectory)
					this.Logger.Info("Logos path found: {0}", logosDirectory);
				else
					this.Logger.Info("Logos path not found");
			}

			foreach (var channel in channels.Items)
			{
				string imagePath = string.Empty;

				if (this.Config.UseServerLogos)
				{
					imagePath = channel.ChannelLogo;
                    channel.ServerLogo = true;
				}
				else
				{
					if (haLogosDirectory)
					{
						string channelLogo = FileSystem.SanitizePath(channel.Name);
						imagePath = this.GetImagePath(logosDirectory, channelLogo);
					}
				}
				channel.IsHD = channel.Name.IndexOf("hd", StringComparison.OrdinalIgnoreCase) >= 0;
				channel.ChannelLogo = imagePath;
				channel.HasChannelLogo = !string.IsNullOrEmpty(channel.ChannelLogo);
			}

			return channels;
		}

		/// <summary> Play channel asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the play channel. </returns>
		public Task<ChannelStream> PlayChannelAsync(PlayChannelRequest request, CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<ChannelStream, PlayChannelRequest>(request, cancellationToken, "Play Channel");
		}

		/// <summary> Stops channel asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the stop channel. </returns>
		public Task<Response> StopChannelAsync(StopChannelRequest request, CancellationToken cancellationToken)
		{
			return this.GetResponseAsync<StopChannelRequest>(request, cancellationToken, "Stop Channel", false);
		}

		#endregion

		#region [Favorites]

		/// <summary> Gets favorites asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the favorites. </returns>
		public Task<Favorites> GetFavoritesAsync(CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<Favorites, FavoritesRequest>(new FavoritesRequest(), cancellationToken, "Get Favorites");
		}
		#endregion

		#region [Epg]

		/// <summary> Gets epg asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the epg. </returns>
		public Task<EpgSearcher> GetEpgAsync(EpgRequest request, CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<EpgSearcher, EpgRequest>(request, cancellationToken, "Get Epg");
		}
		#endregion

		#region Schedules

		/// <summary> Adds a schedule asynchronous to 'cancellationToken'. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the add schedule. </returns>
		public Task<Response> AddScheduleAsync(ScheduleAddRequest request, CancellationToken cancellationToken)
		{
			return this.GetResponseAsync<ScheduleAddRequest>(request, cancellationToken, "Add Schedule", false);
		}

		/// <summary> Gets schedules asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the schedules. </returns>
		public Task<Schedules> GetSchedulesAsync(CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<Schedules, SchedulesRequest>(new SchedulesRequest(), cancellationToken, "Get Schedules");
		}

		/// <summary> Updates the schedule asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the update schedule. </returns>
		public Task<Response> UpdateScheduleAsync(ScheduleUpdater request, CancellationToken cancellationToken)
		{
			return this.GetResponseAsync<ScheduleUpdater>(request, cancellationToken, "Update Schedule", false);
		}

		/// <summary> Removes the schedule asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the remove schedule. </returns>
		public Task<Response> RemoveScheduleAsync(ScheduleRemover request, CancellationToken cancellationToken)
		{
			return this.GetResponseAsync<ScheduleRemover>(request, cancellationToken, "Update Schedule", false);
		}
		#endregion

		#region [Recordings]

		/// <summary> Gets recordings asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the recordings. </returns>
		public Task<Recordings> GetRecordingsAsync(CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<Recordings, RecordingsRequest>(new RecordingsRequest(), cancellationToken, "Get Recordings");
		}

		/// <summary> Removes the recording asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the remove recording. </returns>
		public Task<Response> RemoveRecordingAsync(RecordingRemover request, CancellationToken cancellationToken)
		{
			return this.GetResponseAsync<RecordingRemover>(request, cancellationToken, "Remove Recording", false);
		}

		/// <summary> Stops recording asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the stop recording. </returns>
		public Task<Response> StopRecordingAsync(RecordingStopRequest request, CancellationToken cancellationToken)
		{
			return this.GetResponseAsync<RecordingStopRequest>(request, cancellationToken, "Stop Recording", false);
		}

		#endregion

		#region [Parental]

		/// <summary> Sets parental lock asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the set parental lock. </returns>
		public Task<ParentalStatus> SetParentalLockAsync(ParentalLock request, CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<ParentalStatus, ParentalLock>(request, cancellationToken, "Set Parental Lock");
		}

		/// <summary> Gets parental status asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the parental status. </returns>
		public Task<ParentalStatus> GetParentalStatusAsync(ParentalStatusRequest request, CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<ParentalStatus, ParentalStatusRequest>(request, cancellationToken, "Get Parental Status");
		}

		#endregion

		#region [Server]

		/// <summary> Gets recording settings asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the recording settings. </returns>
		public Task<RecordingSettings> GetRecordingSettingsAsync(CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<RecordingSettings, RecordingSettingsRequest>(new RecordingSettingsRequest(), cancellationToken, "Get Recording Settings");
		}

		/// <summary> Updates the recording settings asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the update recording settings. </returns>
		public Task<Response> UpdateRecordingSettingsAsync(RecordingSettingsUpdater request, CancellationToken cancellationToken)
		{
			return this.GetResponseAsync<RecordingSettingsUpdater>(request, cancellationToken, "Update Recording Settings", false);
		}

		/// <summary> Gets server information asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the server information. </returns>
		public Task<ServerInfo> GetServerInfoAsync(CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<ServerInfo, ServerInfoRequest>(new ServerInfoRequest(), cancellationToken, "Get Server Info");
		}

		/// <summary> Gets streaming capabilities asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the streaming capabilities. </returns>
		public Task<StreamingCapabilities> GetStreamingCapabilitiesAsync(CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<StreamingCapabilities, StreamingCapabilitiesRequest>(new StreamingCapabilitiesRequest(), cancellationToken, "Get Streaming Capabilities");
		}

		/// <summary> Gets streaming capabilities internal asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the streaming capabilities internal. </returns>
		private Task<ResponseObject<StreamingCapabilities, StreamingCapabilitiesRequest>> GetStreamingCapabilitiesInternalAsync(CancellationToken cancellationToken)
		{
			return this.HttpClient.GetResponseObjectAsync<StreamingCapabilities, StreamingCapabilitiesRequest>(new StreamingCapabilitiesRequest(), cancellationToken);
		}

		/// <summary> Gets server information internal asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the server information internal. </returns>
		private Task<ResponseObject<ServerInfo, ServerInfoRequest>> GetServerInfoInternalAsync(CancellationToken cancellationToken)
		{
			return this.HttpClient.GetResponseObjectAsync<ServerInfo, ServerInfoRequest>(new ServerInfoRequest(), cancellationToken);
		}
		#endregion

		#region [Object]

		/// <summary> Removes the object asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the remove object. </returns>
		public Task<Response> RemoveObjectAsync(ObjectRemoverRequest request, CancellationToken cancellationToken)
		{
			return this.GetResponseAsync<ObjectRemoverRequest>(request, cancellationToken, "Remove Object", false);
		}

		/// <summary> Gets objects asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the objects. </returns>
		public Task<Objects> GetObjectsAsync(ObjectsRequest request, CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<Objects, ObjectsRequest>(request, cancellationToken, "Get Objects");
		}

		/// <summary> Gets objects built in asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the objects built in. </returns>
		public Task<Objects> GetObjectsBuiltInAsync(CancellationToken cancellationToken)
		{
			var request = new ObjectsRequest(this.Config.ServerIp, string.Empty)
			{
				ObjectType = EnumObjectType.OBJECT_UNKNOWN,
				ItemType = EnumItemType.ITEM_UNKNOWN,
				RequestedCount = -1,
				ChildrenRequest = true
			};

			return this.GetResponseObjectAsync<Objects, ObjectsRequest>(request, cancellationToken, "Get Objects BuiltIn");
		}

		/// <summary> Gets objects root container asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the objects root container. </returns>
		public async Task<ObjectContainer> GetObjectsRootContainerAsync(CancellationToken cancellationToken)
		{
			var srvObject = await this.GetObjectsBuiltInAsync(cancellationToken).ConfigureAwait(false);
			var objSource = srvObject.Containers.FirstOrDefault(n => n.ContainerType == EnumContainerType.CONTAINER_SOURCE && !string.IsNullOrEmpty(n.ObjectId) && n.SourceId == ObjectsRequest.ContainerBuiltIn);
			return objSource;
		}

		/// <summary> Gets objects recordings by name asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the objects recordings by name. </returns>
		public Task<Objects> GetObjectsRecordingsByNameAsync(CancellationToken cancellationToken)
		{
			return this.GetObjectsByContainerIdInternalAsync(ObjectsRequest.ContainerRecordingsByName, cancellationToken, "Get Objects Recordings By Name");
		}

		/// <summary> Gets objects recordings by date asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the objects recordings by date. </returns>
		public Task<Objects> GetObjectsRecordingsByDateAsync(CancellationToken cancellationToken)
		{
			return this.GetObjectsByContainerIdInternalAsync(ObjectsRequest.ContainerRecordingsByDate, cancellationToken, "Get Objects Recordings By Date");
		}

		/// <summary> Gets objects by genre asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the objects by genre. </returns>
		public Task<Objects> GetObjectsByGenreAsync(CancellationToken cancellationToken)
		{
			return this.GetObjectsByContainerIdInternalAsync(ObjectsRequest.ContainerByGenres, cancellationToken, "Get Objects By Genre");
		}

		/// <summary> Gets objects by series asynchronous. </summary>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the objects by series. </returns>
		public Task<Objects> GetObjectsBySeriesAsync(CancellationToken cancellationToken)
		{
			return this.GetObjectsByContainerIdInternalAsync(ObjectsRequest.ContainerBySeries, cancellationToken, "Get Objects By Series");
		}

		/// <summary> Gets objects by container identifier asynchronous. </summary>
		/// <param name="containerid">		 The containerid. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <param name="childrenRequest">   (Optional) True to children request. </param>
		/// <returns> An asynchronous result that yields the objects by container identifier. </returns>
		public Task<Objects> GetObjectsByContainerIdAsync(string containerid, CancellationToken cancellationToken, bool childrenRequest = false)
		{
			return this.GetObjectsByContainerIdInternalAsync(containerid, cancellationToken, "Get Objects By Container Id", false, childrenRequest);
		}

		/// <summary> Gets object item asynchronous. </summary>
		/// <param name="id">				 The identifier. </param>
		/// <param name="itemType">			 Type of the item. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the object item. </returns>
		public async Task<ObjectItem> GetObjectItemAsync(string id, EnumItemType itemType, CancellationToken cancellationToken)
		{
			var result = await this.GetObjectsByContainerIdInternalAsync(id, cancellationToken, "Get Object Item", false, false, itemType).ConfigureAwait(false);
			return result?.Items != null ? result.Items : null;
		}

		/// <summary> Gets object item recorded TV asynchronous. </summary>
		/// <param name="id">				 The identifier. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the object item recorded TV. </returns>
		public async Task<List<ObjectItemRecordedTV>> GetObjectItemRecordedTVAsync(string id, CancellationToken cancellationToken)
		{
			var result = await this.GetObjectsByContainerIdInternalAsync(id, cancellationToken, "Get Object Item RecordedTV", false, false, EnumItemType.ITEM_RECORDED_TV).ConfigureAwait(false);
			return result?.Items?.RecordedTv != null ? result.Items.RecordedTv : null;
		}

		/// <summary> Gets object item video asynchronous. </summary>
		/// <param name="id">				 The identifier. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the object item video. </returns>
		public async Task<List<ObjectItemVideo>> GetObjectItemVideoAsync(string id, CancellationToken cancellationToken)
		{
			var result = await this.GetObjectsByContainerIdInternalAsync(id, cancellationToken, "Get Object Item Video", false, false, EnumItemType.ITEM_VIDEO).ConfigureAwait(false);
			return result?.Items?.Video != null ? result.Items.Video : null;
		}

		/// <summary> Gets objects tree asynchronous. </summary>
		/// <param name="containerid">		 The containerid. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the objects tree. </returns>
		public async Task<ResponseObject<Objects>> GetObjectsTreeAsync(string containerid, CancellationToken cancellationToken)
		{
			var request = new ObjectsRequest(this.Config.ServerIp, containerid)
			{
				ObjectType = EnumObjectType.OBJECT_UNKNOWN,
				ItemType = EnumItemType.ITEM_UNKNOWN,
				RequestedCount = -1,
				ChildrenRequest = true
			};
			var obj = await this.HttpClient.GetResponseObjectAsync<Objects, ObjectsRequest>(request, cancellationToken).ConfigureAwait(false);
			if (obj.Status == EnumStatusCode.STATUS_OK && obj.ResultObject?.Containers?.Count > 0)
			{
				obj.ResultObject.Nodes = new List<Objects>();
				foreach (ObjectContainer container in obj.ResultObject.Containers)
				{
					var child = await this.GetObjectsTreeAsync(container.ObjectId, cancellationToken).ConfigureAwait(false);
					if (child?.Status == EnumStatusCode.STATUS_OK && child.ResultObject != null)
						obj.ResultObject.Nodes.Add(child.ResultObject);
				}
			}

			return obj;
		}

		#endregion

		#region [Multicast]

		/// <summary> Gets multicast URL asynchronous. </summary>
		/// <param name="request">			 The request. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the multicast URL. </returns>
		public Task<Multicast> GetMulticastUrlAsync(MulticastRequest request, CancellationToken cancellationToken)
		{
			return this.GetResponseObjectAsync<Multicast, MulticastRequest>(request, cancellationToken, "Multicast Url", false);
		}

		#endregion

		#region [Private methods]

		/// <summary> Ensures that connection asynchronous. </summary>
		/// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result. </returns>
		private async Task EnsureConnectionAsync(CancellationToken cancellationToken)
		{
			if (!this.Status.HasConnectionError && this.Status.ConnectionErrorCount < _ConnectionErrorRetry)
			{
				if (string.IsNullOrEmpty(this.Config.ServerIp))
				{
					string message = "The ServerIp must be configured.";
					this.Status.ErrorMessage = message;
					this._Logger.Error(message);
					throw new InvalidOperationException(message);
				}

				if (string.IsNullOrEmpty(this.Config.ServerPort.ToString()))
				{
					string message = "The ServerPort must be configured.";
					this.Status.ErrorMessage = message;
					this._Logger.Error(message);
					throw new InvalidOperationException(message);
				}

				try
				{
					if (!this.Status.IsAvailable)
					{
						this._Logger.Info("Connecting to server : {0}", this.Config.ApiUrl);

						ResponseObject<ServerInfo, ServerInfoRequest> result = await this.GetServerInfoInternalAsync(cancellationToken).ConfigureAwait(true);

						if (result.Status == EnumStatusCode.STATUS_NOT_IMPLEMENTED)
						{
							var strCapResult = await this.GetStreamingCapabilitiesInternalAsync(cancellationToken).ConfigureAwait(true);
							if (strCapResult.Status == EnumStatusCode.STATUS_OK)
							{
								this._Logger.Info("Connected to server using streaming capabilities");
								this.Status.Info = new ServerInfo();
								this.Status.HasConnectionError = false;
								this.Status.IsAvailable = true;
								this.Status.Capabilities = strCapResult.ResultObject;
							}
							else
							{
								throw new InvalidOperationException(this.FormatDVBLinkStatusError(result.Status), result.Exception);
							}
						}
						else if (result.Status != EnumStatusCode.STATUS_OK && result.Status != EnumStatusCode.STATUS_NOT_IMPLEMENTED)
						{
							this.Status.IsAvailable = false;
							this.Status.ConnectionErrorCount++;
							this.Status.HasConnectionError = this.Status.ConnectionErrorCount > _ConnectionErrorRetry;
							throw new InvalidOperationException(this.FormatDVBLinkStatusError(result.Status), result.Exception);
						}
						else
						{
							this._Logger.Info("Server version : {0}", result.ResultObject.Version);

							this._Logger.Info("Connected to server using server version info");
							this.Status.Info = result.ResultObject;
							this.Status.HasConnectionError = false;
							this.Status.IsAvailable = true;
							var cap = await this.GetStreamingCapabilitiesAsync(cancellationToken).ConfigureAwait(true);
							if (cap != null)
							{
								this.Status.Capabilities = cap;
							}
							var versionString = this.Status.Info.Version.Split('.')?[0];
							int.TryParse(versionString, out int version);
							this.Status.Version = version;
						}
					}
				}
				catch (Exception ex)
				{
					this._Logger.ErrorException("It's not possible to Ensure a connection to the Server", ex);
				}
			}
		}

		/// <summary> Gets objects by container identifier internal asynchronous. </summary>
		/// <param name="containerid">		 The containerid. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <param name="message">			 (Optional) The message. </param>
		/// <param name="addServerObjectid"> (Optional) True to add server objectid. </param>
		/// <param name="childrenRequest">   (Optional) True to children request. </param>
		/// <param name="itemType">			 (Optional) Type of the item. </param>
		/// <returns> An asynchronous result that yields the objects by container identifier internal. </returns>
		private async Task<Objects> GetObjectsByContainerIdInternalAsync(string containerid, CancellationToken cancellationToken, string message = "", bool addServerObjectid = true, bool childrenRequest = true, EnumItemType itemType = EnumItemType.ITEM_UNKNOWN)
		{
			var srvObject = await this.GetObjectsRootContainerAsync(cancellationToken).ConfigureAwait(false);
			string id = addServerObjectid ? srvObject.ObjectId + containerid : containerid;
			var request = new ObjectsRequest(this.Config.ServerIp, id)
			{
				ObjectType = EnumObjectType.OBJECT_UNKNOWN,
				ItemType = itemType,
				RequestedCount = -1,
				ChildrenRequest = childrenRequest
			};
			return await this.GetResponseObjectAsync<Objects, ObjectsRequest>(request, cancellationToken, message).ConfigureAwait(false);
		}

		/// <summary> Gets image path. </summary>
		/// <param name="folder">	   Pathname of the folder. </param>
		/// <param name="channelName"> Name of the channel. </param>
		/// <returns> The image path. </returns>
		private string GetImagePath(string folder, string channelName)
		{
			string imagePath = string.Empty;
            if (!string.IsNullOrEmpty(folder) && !string.IsNullOrEmpty(channelName))
			{
                string testPath = Path.Combine(folder, channelName + ".png");
                if (File.Exists(testPath))
				{
					imagePath = testPath;
				}

				testPath = Path.Combine(folder, channelName + ".jpg");
				if (File.Exists(testPath))
				{
					imagePath = testPath;
				}
			}
			return imagePath;
		}

		#endregion

	}
}
