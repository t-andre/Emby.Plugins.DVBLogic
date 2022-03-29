// <copyright file="IPluginConfiguration.cs" >
// Copyright (c) 2018 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares</author>
// <date>29.08.2018</date>
// <summary>Declares the IPluginConfiguration interface</summary>
namespace Emby.Plugins.DVBLogic.Configuration
{
	/// <summary> Interface for plugin configuration. </summary>
	public interface IPluginConfiguration
	{
		#region [Properties Implementation]

		/// <summary> Gets or sets options for controlling the other. </summary>
		/// <value> Options that control the other. </value>
		OtherOptions OtherOptions { get; set; }

		/// <summary> Gets or sets the server configuration. </summary>
		/// <value> The server configuration. </value>
		TSoft.TVServer.Configurations.ServerConfiguration ServerConfig { get; set; }

		/// <summary> Gets or sets the server status. </summary>
		/// <value> The server status. </value>
		TSoft.TVServer.Configurations.ServerStatus ServerStatus { get; set; }
		#endregion

		#region [Methods Implementation]
		/// <summary> Stops channel live TV. </summary>
		void StopChannelLiveTV();

		/// <summary> Stops client live TV. </summary>
		void StopClientLiveTV();
		#endregion
	}
}
