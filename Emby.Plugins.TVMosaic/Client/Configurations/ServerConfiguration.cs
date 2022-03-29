// <copyright file="Configuration.cs" >
// Copyright (c) 2015 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>18.12.2015</date>
// <summary>Implements the configuration class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSoft.TVServer.Configurations
{
    /// <summary> A server configuration. </summary>
    public class ServerConfiguration
    {
        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Configurations.ServerConfiguration
        /// class. </summary>
        public ServerConfiguration():this("localhost", 8100, 8101)
        {
       
        }

		/// <summary> Initializes a new instance of the TSoft.TVServer.Configurations.ServerConfiguration class. </summary>
		/// <param name="serverIp">		 The server IP. </param>
		/// <param name="serverPort">    The server port. </param>
		/// <param name="streamingPort"> The streaming port. </param>
		public ServerConfiguration(string serverIp, int serverPort, int streamingPort)
		{
			this.ServerIp = serverIp;
			this.ServerPort = serverPort;
			this.StreamingPort = streamingPort;
			this.EnableTimeshift = false;
			this.EnableDebugLogging = false;
			this.UseAuthorization = false;
			this.DirectStreamRequest = true;
			this.UseServerLogos = false;
			this.Enable = true;
		}
		#endregion

		#region [Public Properties]
		/// <summary> Gets URL of the API. </summary>
		/// <value> The API URL. </value>
		public string ApiUrl
        {
            get { return $"http://{this.ServerIp}:{this.ServerPort}/mobile/"; }
        }

        /// <summary> Gets URL of the base. </summary>
        /// <value> The base URL. </value>
        public string BaseUrl
        {
            get { return $"http://{this.ServerIp}:{this.ServerPort}/"; }
        }

		/// <summary> Gets URL of the streaming. </summary>
		/// <value> The streaming URL. </value>
		public string StreamingUrl
		{
			get { return $"http://{this.ServerIp}:{this.StreamingPort}/"; }
		}

		/// <summary> Gets or sets a value indicating whether the direct stream request. </summary>
		/// <value> true if direct stream request, false if not. </value>
		public bool DirectStreamRequest { get; set; }

        /// <summary> Gets or sets a value indicating whether the debug logging is enabled. </summary>
        /// <value> true if enable debug logging, false if not. </value>
        public Boolean EnableDebugLogging { get; set; }

        /// <summary> Gets or sets a value indicating whether the timeshift is enabled. </summary>
        /// <value> true if enable timeshift, false if not. </value>
        public Boolean EnableTimeshift { get; set; }

        /// <summary> Gets or sets the password. </summary>
        /// <value> The password. </value>
        public string Password { get; set; }

        /// <summary> Gets or sets the server IP. </summary>
        /// <value> The server IP. </value>
        public string ServerIp { get; set; }

        /// <summary> Gets or sets the full pathname of the logos file. </summary>
        /// <value> The full pathname of the logos file. </value>
        public string LogosPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// TSoft.TVServer.Configurations.ServerConfiguration use server logos. </summary>
        /// <value> true if use server logos, false if not. </value>
        public bool UseServerLogos { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// TSoft.TVServer.Configurations.ServerConfiguration use server recording paddings. </summary>
        /// <value> true if use server recording paddings, false if not. </value>
        public bool UseServerRecordingPaddings { get; set; }

        /// <summary> Gets or sets the server port. </summary>
        /// <value> The server port. </value>
        public int ServerPort { get; set; }

		/// <summary> Gets or sets the streaming port. </summary>
		/// <value> The streaming port. </value>
		public int StreamingPort { get; set; }

		/// <summary> Gets or sets a value indicating whether the transcode. </summary>
		/// <value> true if transcode, false if not. </value>
		public bool Transcode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// TSoft.TVServer.Configurations.ServerConfiguration use authorization. </summary>
        /// <value> true if use authorization, false if not. </value>
        public bool UseAuthorization { get; set; }

        /// <summary> Gets or sets the name of the user. </summary>
        /// <value> The name of the user. </value>
        public string UserName { get; set; }

        /// <summary> Gets or sets the duration of the live TV. </summary>
        /// <value> The live TV duration. </value>
        public long LiveTVDuration { get; set; }

		/// <summary> Gets or sets a value indicating whether this ServerConfiguration is enabled. </summary>
		/// <value> True if enable, false if not. </value>
		public bool Enable { get; set; }

		#endregion

	}
}
