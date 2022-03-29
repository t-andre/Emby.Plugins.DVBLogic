using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSoft.TVServer.Entities;

namespace TSoft.TVServer.Configurations
{
	/// <summary> A server status. </summary>
	public class ServerStatus
	{
		#region [Public properties]

		/// <summary> Gets or sets the capabilities. </summary>
		/// <value> The capabilities. </value>
		public StreamingCapabilities Capabilities { get; set; } = new StreamingCapabilities();

		/// <summary> Gets or sets the number of connection errors. </summary>
		/// <value> The number of connection errors. </value>
		public int ConnectionErrorCount { get; set; }

		/// <summary> Gets or sets the version. </summary>
		/// <value> The version. </value>
		public int Version { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this
		/// TSoft.TVServer.Configurations.ServerStatus has connection error. </summary>
		/// <value>
		/// true if this TSoft.TVServer.Configurations.ServerStatus has connection error, false if
		/// not. </value>
		public bool HasConnectionError { get; set; }

		/// <summary> Gets or sets the information. </summary>
		/// <value> The information. </value>
		public ServerInfo Info { get; set; } = new ServerInfo();

		/// <summary>
		/// Gets or sets a value indicating whether this
		/// TSoft.TVServer.Configurations.ServerStatus is available. </summary>
		/// <value>
		/// true if this TSoft.TVServer.Configurations.ServerStatus is available, false if not. </value>
		public bool IsAvailable { get; set; }

		/// <summary> Gets or sets a message describing the error. </summary>
		/// <value> A message describing the error. </value>
		public string ErrorMessage { get; set; }

		#endregion

		#region [Public methods]

		/// <summary> Reinitializes this TSoft.TVServer.Configurations.ServerStatus. </summary>
		public void Reinitialize()
		{
			this.Capabilities = new StreamingCapabilities();
			this.Info = new ServerInfo();
			this.IsAvailable = false;
			this.ConnectionErrorCount = 0;
			this.HasConnectionError = false;
			this.Version = 0;
			this.ErrorMessage = string.Empty;
		}
		#endregion

	}
}
