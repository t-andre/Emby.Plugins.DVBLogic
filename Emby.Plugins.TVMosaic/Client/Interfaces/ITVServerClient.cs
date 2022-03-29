using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TSoft.TVServer.Configurations;
using TSoft.TVServer.Constants;
using TSoft.TVServer.Entities;
using TSoft.TVServer.Helpers;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer
{
	/// <summary> Interface for itv server client. </summary>
	public interface ITVServerClient
	{
		#region [Properties Implementation]

		/// <summary> Gets the configuration. </summary>
		/// <value> The configuration. </value>
		ServerConfiguration Config { get; }

		/// <summary> Gets or sets a value indicating whether the debug log. </summary>
		/// <value> true if debug log, false if not. </value>
		bool DebugLog { get; set; }

		/// <summary> Gets the HTTP client. </summary>
		/// <value> The HTTP client. </value>
		HttpClientBase HttpClient { get; }

		/// <summary> Gets the identifier of the client. </summary>
		/// <value> The identifier of the client. </value>
		string ClientId { get; }

		/// <summary> The logger. </summary>
		/// <value> The logger. </value>
		AbstractLogger Logger { get; }

		/// <summary> Gets the manager for session. </summary>
		/// <value> The session manager. </value>
		SessionManager SessionManager { get; }

		/// <summary> Gets the status. </summary>
		/// <value> The status. </value>
		ServerStatus Status { get; }

		/// <summary> Gets or sets information describing the tuner. </summary>
		/// <value> Information describing the tuner. </value>
		TunerInfo TunerInfo { get; set; }

		/// <summary> Gets a value indicating whether this ITVServerClient is enabled. </summary>
		/// <value> True if enable, false if not. </value>
		bool Enable { get; }

		/// <summary> Gets the type of the client. </summary>
		/// <value> The type of the client. </value>
		EnumTVServerClientType ClientType { get; }

		/// <summary> Gets the name. </summary>
		/// <value> The name. </value>
		string Name { get; }

		#endregion

		#region [Methods Implementation]
		/// <summary> Initializes this TSoft.TVServer.DVBLinkClient. </summary>
		/// <param name="logger"> The logger. </param>
		/// <param name="config"> (Optional) The configuration. </param>
		/// <param name="httpClient"> (Optional) The HTTP client. </param>
		/// <returns> true if it succeeds, false if it fails. </returns>
		bool Initialize(AbstractLogger logger, ServerConfiguration config = null, HttpClientBase httpClient = null);

		/// <summary> Updates the configuration described by config. </summary>
		/// <param name="config"> The configuration. </param>
		/// <returns> An IDVBLinkClient. </returns>
		ITVServerClient UpdateConfig(ServerConfiguration config);

		/// <summary> Updates the logger described by logger. </summary>
		/// <param name="logger"> The logger. </param>
		/// <returns> An IDVBLinkClient. </returns>
		ITVServerClient UpdateLogger(AbstractLogger logger);
		#endregion

	}
}
