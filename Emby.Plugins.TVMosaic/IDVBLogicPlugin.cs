// <copyright file="IDVBLogicPlugin.cs" >
// Copyright (c) 2018 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares</author>
// <date>29.08.2018</date>
// <summary>Implements the idvb logic plugin class</summary>
using System;
using TSoft.TVServer.Helpers;
using TSoft.TVServer.Constants;
using MediaBrowser.Model.Plugins;
using Emby.Plugins.DVBLogic.Proxies;
using Emby.Plugins.DVBLogic.Configuration;

namespace Emby.Plugins.DVBLogic
{
	/// <summary> Interface for idvb logic plugin. </summary>
	public interface IDVBLogicPlugin
	{
		#region [Properties Implementation]

		/// <summary> Gets the name. </summary>
		/// <value> The name. </value>
		string Name { get; }

		/// <summary> Gets or sets the configuration. </summary>
		/// <value> The configuration. </value>
		PluginConfigurationBase Config { get; }

		/// <summary> Gets the filename of the configuration file. </summary>
		/// <value> The filename of the configuration file. </value>
		string ConfigurationFileName { get; }

		/// <summary> Gets or sets the logger. </summary>
		/// <value> The logger. </value>
		AbstractLogger Logger { get; }

		/// <summary> Gets or sets the TV proxy. </summary>
		/// <value> The TV proxy. </value>
		TVServiceProxy TVProxy { get; }
		#endregion

		#region [Methods Implementation]

		/// <summary> Updates the configuration described by configuration. </summary>
		/// <param name="configuration"> The configuration. </param>
		void UpdateConfiguration(BasePluginConfiguration configuration);
		#endregion

	}
}
