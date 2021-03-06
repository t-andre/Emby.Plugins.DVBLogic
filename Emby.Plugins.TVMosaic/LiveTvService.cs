// <copyright file="LiveTvService.cs" >
// Copyright (c) 2018 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares</author>
// <date>23.08.2018</date>
// <summary>Implements the live TV service class</summary>
using Emby.Plugins.DVBLogic;
using MediaBrowser.Controller;
using MediaBrowser.Controller.LiveTv;
using MediaBrowser.Controller.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emby.Plugins.TVMosaic
{
	/// <summary> A live TV service. </summary>
	/// <seealso cref="T:Emby.Plugins.DVBLink.LiveTvServiceBase"/>
	public class LiveTvService : LiveTvServiceBase, ILiveTvService
	{
		#region [Constructors]
		/// <summary> Initializes a new instance of the Emby.Plugins.TVMosaic.LiveTvService class. </summary>
		/// <param name="serverApplicationPaths"> The server application paths. </param>
		/// <param name="sessionManager">		  Manager for session. </param>
		public LiveTvService(IServerApplicationPaths serverApplicationPaths, ISessionManager sessionManager) : base(Plugin.Instance, serverApplicationPaths, sessionManager)
		{
		}
		#endregion

		#region [Public properties]
		/// <summary> Gets URL of the home page. </summary>
		/// <value> The home page URL. </value>
		/// <seealso cref="P:Emby.Plugins.DVBLink.LiveTvServiceBase.HomePageUrl"/>
		public override string HomePageUrl
		{
            // Logo 1 : https://tv-mosaic.com/img/media/tvmosaic_leanback_1280_720.png
            get { return "http://tv-mosaic.com"; }
		}

		/// <summary> Gets the name. </summary>
		/// <value> The name. </value>
		/// <seealso cref="P:Emby.Plugins.DVBLink.LiveTvServiceBase.Name"/>
		public override string Name { get { return this.DVBLogicPlugin.Name; } }
		#endregion
	}
}
