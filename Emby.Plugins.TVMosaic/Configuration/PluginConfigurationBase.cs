// <copyright file="PluginConfigurationBase.cs" >
// Copyright (c) 2018 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares</author>
// <date>30.08.2018</date>
// <summary>Implements the plugin configuration base class</summary>
using MediaBrowser.Model.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Emby.Plugins.DVBLogic.Configuration
{
	/// <summary> A plugin configuration base. </summary>
	/// <seealso cref="T:MediaBrowser.Model.Plugins.BasePluginConfiguration"/>
	/// <seealso cref="T:Emby.Plugins.DVBLogic.Configuration.IPluginConfiguration"/>
	public abstract class PluginConfigurationBase : BasePluginConfiguration, IPluginConfiguration
	{
		#region [Constructors]
		/// <summary> Initializes a new instance of the Emby.Plugins.DVBLink.Configuration.PluginConfiguration class. </summary>
		public PluginConfigurationBase()
		{

		}
		#endregion

		#region [Public properties]

		/// <summary> Gets or sets options for controlling the other. </summary>
		/// <value> Options that control the other. </value>
		/// <seealso cref="P:Emby.Plugins.DVBLogic.Configuration.IPluginConfiguration.OtherOptions"/>
		public OtherOptions OtherOptions { get; set; } = new OtherOptions();

		/// <summary> Gets or sets the server configuration. </summary>
		/// <value> The server configuration. </value>
		/// <seealso cref="P:Emby.Plugins.DVBLogic.Configuration.IPluginConfiguration.ServerConfig"/>
		public TSoft.TVServer.Configurations.ServerConfiguration ServerConfig { get; set; }

		/// <summary> Gets or sets the server status. </summary>
		/// <value> The server status. </value>
		/// <seealso cref="P:Emby.Plugins.DVBLogic.Configuration.IPluginConfiguration.ServerStatus"/>
		[XmlIgnore]
		public TSoft.TVServer.Configurations.ServerStatus ServerStatus { get; set; } = new TSoft.TVServer.Configurations.ServerStatus();
		#endregion

		#region [Public methods]

		/// <summary> Stops channel live TV. </summary>
		/// <seealso cref="M:Emby.Plugins.DVBLogic.Configuration.IPluginConfiguration.StopChannelLiveTV()"/>
		public void StopChannelLiveTV()
		{

		}

		/// <summary> Stops client live TV. </summary>
		/// <seealso cref="M:Emby.Plugins.DVBLogic.Configuration.IPluginConfiguration.StopClientLiveTV()"/>
		public void StopClientLiveTV()
		{

		}
		#endregion
	}
}
