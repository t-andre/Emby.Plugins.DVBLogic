// <copyright file="PluginConfiguration.cs" >
// Copyright (c) 2017 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>01.09.2017</date>
// <summary>Implements the plugin configuration class</summary>
using Emby.Plugins.DVBLogic.Configuration;
using MediaBrowser.Model.Plugins;
using System.Xml.Serialization;

namespace Emby.Plugins.TVMosaic.Configuration
{
    /// <summary> A plugin configuration. </summary>
    /// <seealso cref="T:MediaBrowser.Model.Plugins.BasePluginConfiguration"/>
    public class PluginConfiguration : PluginConfigurationBase
    {
        #region [Constructors]
        /// <summary> Initializes a new instance of the Emby.Plugins.TVMosaic.Configuration.PluginConfiguration class. </summary>
        public PluginConfiguration()
        {
            this.ServerConfig = new TSoft.TVServer.Configurations.ServerConfiguration("localhost", 9270, 9271);
        }
        #endregion
    }
}
