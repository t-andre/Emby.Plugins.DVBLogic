// <copyright file="Plugin.cs" >
// Copyright (c) 2017 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>01.09.2017</date>
// <summary>Implements the TVMosaic plugin class</summary>
using System;
using System.Collections.Generic;
using Emby.Plugins.DVBLogic.Proxies;
using TSoft.TVServer.Helpers;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Serialization;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using TSoft.TVServer.Constants;
using Emby.Plugins.TVMosaic.Configuration;
using Emby.Plugins.DVBLogic;
using Emby.Plugins.DVBLogic.Helpers;
using Emby.Plugins.DVBLogic.Configuration;
using MediaBrowser.Model.Drawing;
using System.IO;

namespace Emby.Plugins.TVMosaic
{
	/// <summary> A plugin. </summary>
	/// <seealso cref="T:MediaBrowser.Common.Plugins.BasePlugin{Emby.Plugins.DVBLink.Configuration.PluginConfiguration}"/>
	/// <seealso cref="T:MediaBrowser.Model.Plugins.IHasWebPages"/>
	public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages, IDVBLogicPlugin, IHasThumbImage
    {
		#region [Constructors]
		/// <summary> Initializes a new instance of the Emby.Plugins.TVMosaic.Plugin class. </summary>
		/// <param name="applicationPaths"> The application paths. </param>
		/// <param name="xmlSerializer">    The XML serializer. </param>
		/// <param name="logger">		    The logger. </param>
		/// <param name="httpClient">	    The HTTP client. </param>
		public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, ILogger logger, IHttpClient httpClient)
			: base(applicationPaths, xmlSerializer)
		{
			Instance = this;
			this.Logger = new PluginLogger(logger, this.Name);
			this.TVProxy = new TVServiceProxy(this, EnumTVServerClientType.TVMosaic, httpClient, xmlSerializer);
		}
		#endregion

		#region [Fields]
		/// <summary> The identifier. </summary>
		private Guid _Id = new Guid("864e544d-051b-4b4c-aeec-20d42773c796");
		#endregion

		#region [Public properties]
		/// <summary> Gets the instance. </summary>
		/// <value> The instance. </value>
		public static IDVBLogicPlugin Instance { get; private set; }

		/// <summary> Gets or sets the plugin configuration. </summary>
		/// <value> The plugin configuration. </value>
		public PluginConfigurationBase Config { get { return this.Configuration; } }

		/// <summary> Gets or sets the logger. </summary>
		/// <value> The logger. </value>
		public AbstractLogger Logger { get; }

		/// <summary> Gets or sets the TV proxy. </summary>
		/// <value> The TV proxy. </value>
		public TVServiceProxy TVProxy { get; }

		/// <summary> Gets the filename of the configuration file. </summary>
		/// <value> The filename of the configuration file. </value>
		public override string ConfigurationFileName => "Emby.Plugins.TVMosaic.xml";

		/// <summary> Gets the description. </summary>
		/// <value> The description. </value>
		public override string Description
		{
			get { return "Provides live tv using TVMosaic or as a back-end."; }
		}

		/// <summary> Gets the identifier. </summary>
		/// <value> The identifier. </value>
		public override Guid Id
		{
			get { return this._Id; }
		}

		/// <summary> Gets the name. </summary>
		/// <value> The name. </value>
		public override string Name
		{
			get { return "TVMosaic"; }
		}

        /// <summary> Gets thumb image. </summary>
        /// <returns> The thumb image. </returns>
        public Stream GetThumbImage()
        {
            var type = GetType();
            return type.Assembly.GetManifestResourceStream(type.Namespace + ".thumb.png");
        }

        /// <summary> Gets the thumb image format. </summary>
        /// <value> The thumb image format. </value>
        public ImageFormat ThumbImageFormat
        {
            get
            {
                return ImageFormat.Png;
            }
        }
        #endregion

        #region [Public methods]
        /// <summary> Gets the pages in this collection. </summary>
        /// <returns> An enumerator that allows foreach to be used to process the pages in this collection. </returns>
        public IEnumerable<PluginPageInfo> GetPages()
		{
			return new[] { new PluginPageInfo
				{
					Name = this.Name,
					EmbeddedResourcePath = "Emby.Plugins.TVMosaic.Configuration.tvmosaicconfigPage.html",
					EnableInMainMenu = false,
					MenuSection = "Live Tv"
				}
			};
		}

		/// <summary> Updates the configuration described by configuration. </summary>
		/// <param name="configuration"> The configuration. </param>
		public override void UpdateConfiguration(BasePluginConfiguration configuration)
		{
			base.UpdateConfiguration(configuration);
			this.TVProxy.Client.UpdateConfig(((PluginConfiguration)configuration).ServerConfig);
		}
		#endregion
	}
}
