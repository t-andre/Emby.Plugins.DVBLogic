// <copyright file="PluginConfiguration.cs" >
// Copyright (c) 2017 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>01.09.2017</date>
// <summary>Implements the plugin configuration class</summary>
using Emby.Plugins.DVBLogic.Configuration;
using MediaBrowser.Model.Plugins;
using System.Xml.Serialization;

namespace Emby.Plugins.DVBLink.Configuration
{
	/// <summary> A plugin configuration. </summary>
	/// <seealso cref="T:Emby.Plugins.DVBLogic.Configuration.PluginConfigurationBase"/>
	public class PluginConfiguration : PluginConfigurationBase
	{
		public PluginConfiguration()
		{
			this.ServerConfig = new TSoft.TVServer.Configurations.ServerConfiguration("localhost", 8100, 8101);
		}
	}
}
