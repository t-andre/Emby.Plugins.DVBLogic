// <copyright file="ServerApiEndPoints.cs" >
// Copyright (c) 2017 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>01.09.2017</date>
// <summary>Implements the server API end points class</summary>
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Extensions;
using TSoft.TVServer.Entities;
using MediaBrowser.Model.Services;
using TSoft.TVServer.Configurations;
using Emby.Plugins.DVBLogic;

namespace Emby.Plugins.TVMosaic
{
    /// <summary> A get profiles. </summary>
    /// <seealso cref="T:MediaBrowser.Model.Services.IReturn{System.Collections.Generic.List{System.String}}"/>
    /// <seealso cref="T:ServiceStack.IReturn{System.Collections.Generic.List{System.String}}"/>
	[Route("/TVMosaicPlugin/Profiles", "GET", Summary = "Gets a list of streaming profiles")]
    public class GetProfiles : IReturn<List<string>>
    {
    }

    /// <summary> A get channels. </summary>
    /// <seealso cref="T:MediaBrowser.Model.Services.IReturn{System.Collections.Generic.List{System.String}}"/>
    /// <seealso cref="T:ServiceStack.IReturn{System.Collections.Generic.List{System.String}}"/>
    [Route("/TVMosaicPlugin/Channels", "GET", Summary = "Gets a list of channels")]
    public class GetChannels : IReturn<List<string>>
    {
    }

    /// <summary> A get sessions. </summary>
    /// <seealso cref="T:MediaBrowser.Model.Services.IReturn{System.Collections.Generic.List{TSoft.TVServer.Entities.Session}}"/>
    /// <seealso cref="T:ServiceStack.IReturn{System.Collections.Generic.List{TSoft.TVServer.Entities.Session}}"/>
    [Route("/TVMosaicPlugin/Sessions", "GET", Summary = "Gets a list of sessions")]
    public class GetSessions : IReturn<List<Session>>
    {
    }

	/// <summary> A get favourites. </summary>
	/// <seealso cref="T:MediaBrowser.Model.Services.IReturn{System.Collections.Generic.List{TSoft.TVServer.Entities.Favorites}}"/>
	[Route("/TVMosaicPlugin/Favourites", "GET", Summary = "Gets a list of Favourites channels")]
	public class GetFavourites : IReturn<List<Favorites>>
	{
	}

	/// <summary> A get stop session. </summary>
	/// <seealso cref="T:MediaBrowser.Model.Services.IReturn{System.Boolean}"/>
	/// <seealso cref="T:ServiceStack.IReturn{System.Boolean}"/>
	[Route("/TVMosaicPlugin/StopSession", "GET", Summary = "Stop the session")]
    public class GetStopSession : IReturn<Boolean>
    {
        #region [Public properties]
        /// <summary> Gets or sets the identifier of the session. </summary>
        /// <value> The identifier of the session. </value>
        public string SessionId { get; set; }
        #endregion
    }

    /// <summary> A get connection. </summary>
    /// <seealso cref="T:MediaBrowser.Model.Services.IReturn{System.Boolean}"/>
    /// <seealso cref="T:ServiceStack.IReturn{System.Boolean}"/>
    [Route("/TVMosaicPlugin/TestConnection", "GET", Summary = "Tests the connection to DVBLink")]
    public class GetConnection : IReturn<Boolean>
    {
    }

	/// <summary> A get server status. </summary>
	/// <seealso cref="T:MediaBrowser.Model.Services.IReturn{TSoft.TVServer.Configurations.ServerStatus}"/>
	[Route("/TVMosaicPlugin/ServerStatus", "GET", Summary = "Get DVBLink Server Status")]
	public class GetServerStatus : IReturn<ServerStatus>
	{
	}

	/// <summary> A server API end points. </summary>
	/// <seealso cref="T:MediaBrowser.Model.Services.IService"/>
	/// <seealso cref="T:MediaBrowser.Controller.Net.IRestfulService"/>
	public class ServerApiEndPoints : IService
	{
        #region [Public methods]
        /// <summary> Gets an object using the given request. </summary>
        /// <param name="request"> The request to get. </param>
        /// <returns> An object. </returns>
        public object Get(GetSessions request)
        {
            try
            {
                //Plugin.TVProxy.Client.SessionManager.AddSession(new Session { DeviceName = "1", StreamSource = EnumStreamSourceType.LIVETV});
                //Plugin.TVProxy.Client.SessionManager.AddSession(new Session { DeviceName = "2", StreamSource = EnumStreamSourceType.LIVETV });
                //Plugin.TVProxy.Client.SessionManager.AddSession(new Session { DeviceName = "3", StreamSource = EnumStreamSourceType.LIVETV });
                //Plugin.TVProxy.Client.SessionManager.AddSession(new Session { DeviceName = "5", StreamSource = EnumStreamSourceType.LIVETV });
                //Plugin.TVProxy.Client.SessionManager.AddSession(new Session { DeviceName = "6", StreamSource = EnumStreamSourceType.LIVETV });

                var result = Plugin.Instance.TVProxy.Client.SessionManager.GetSessions();
                return result;
            }
            catch (Exception exception)
            {
				Plugin.Instance.Logger.ErrorException("There was an issue testing the API connection", exception);
            }

            return false;
        }

		/// <summary> Gets an object using the given request. </summary>
		/// <param name="request"> The request to get. </param>
		/// <returns> An object. </returns>
		public async Task<object> Get(GetFavourites request)
		{
			try
			{
				Favorites result = await Plugin.Instance.TVProxy.Client.GetFavoritesAsync(new CancellationToken()).ConfigureAwait(false);
				result.Items.Insert(0, new Favorite { Id = "-1", Name = "None" });
				return result;
			}
			catch (Exception exception)
			{
				Plugin.Instance.Logger.ErrorException("There was an issue getting favourites", exception);
			}

			return new Favorites();
		}

		/// <summary> Gets an object using the given request. </summary>
		/// <param name="request"> The request to get. </param>
		/// <returns> An object. </returns>
		public async Task<object> Get(GetServerStatus request)
		{
			try
			{
				return await Task.FromResult<object>(Plugin.Instance.TVProxy.Client.Status).ConfigureAwait(false);
			}
			catch (Exception exception)
			{
				Plugin.Instance.Logger.ErrorException("There was an issue getting Server Status", exception);
			}

			return new ServerStatus();
		}

		/// <summary> Gets an object using the given request. </summary>
		/// <param name="request"> The request to get. </param>
		/// <returns> An object. </returns>
		public async Task<object> GetAsync(GetStopSession request)
        {
            try
            {
                await Plugin.Instance.TVProxy.CloseLiveStreamAsync(request.SessionId, new CancellationToken(), true).ConfigureAwait(false);
                return true;
            }
            catch (Exception exception)
            {
				Plugin.Instance.Logger.ErrorException("There was an issue testing the API connection", exception);
            }

            return false;
        }

        /// <summary> Gets an object using the given request. </summary>
        /// <exception cref="ResourceNotFoundException"> Thrown when a Resource Not Found error condition
        /// occurs. </exception>
        /// <param name="request"> The request to get. </param>
        /// <returns> An object. </returns>
        public async Task<object> GetAsync(GetConnection request)
        {
            try
            {
                var result = await Plugin.Instance.TVProxy.Client.GetServerInfoAsync(new CancellationToken()).ConfigureAwait(false);
                if (result != null)
                    return true;
                else
                    throw new ResourceNotFoundException();
            }
            catch (Exception exception)
            {
				Plugin.Instance.Logger.ErrorException("There was an issue testing the API connection", exception);
            }

            return false;
        }
        #endregion
    }
}
