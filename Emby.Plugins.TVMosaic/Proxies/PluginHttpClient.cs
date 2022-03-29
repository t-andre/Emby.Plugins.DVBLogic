// <copyright file="PluginHttpClient.cs" >
// Copyright (c) 2017 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>12.09.2017</date>
// <summary>Implements the plugin HTTP client class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TSoft.TVServer;
using TSoft.TVServer.Helpers;
using TSoft.TVServer.Configurations;
using System.IO;
using MediaBrowser.Model.Serialization;
using MediaBrowser.Common.Net;

namespace Emby.Plugins.DVBLogic.Proxies
{
	/// <summary> A plugin HTTP client. </summary>
	/// <seealso cref="T:TSoft.TVServer.HttpClientBase"/>
	public class PluginHttpClient : HttpClientBase
	{
		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the Emby.Plugins.DVBLink.Proxies.PluginHttpClient class. </summary>
		/// <param name="logger"> The logger. </param>
		public PluginHttpClient(AbstractLogger logger) : base(logger)
		{

		}

		/// <summary>
		/// Initializes a new instance of the Emby.Plugins.DVBLink.Proxies.PluginHttpClient class. </summary>
		/// <param name="logger"> The logger. </param>
		/// <param name="httpClient"> The HTTP client. </param>
		/// <param name="xmlSerializer"> The XML serializer. </param>
		public PluginHttpClient(AbstractLogger logger, IHttpClient httpClient, IXmlSerializer xmlSerializer) : base(logger)
		{
			HttpClient = httpClient;
			XmlSerializer = xmlSerializer;
		}
		#endregion

		#region [Public properties]
		/// <summary> Gets the XML serializer. </summary>
		/// <value> The XML serializer. </value>
		public IXmlSerializer XmlSerializer { get; private set; }
		#endregion

		#region [Protected properties]
		/// <summary> Gets the HTTP client. </summary>
		/// <value> The HTTP client. </value>
		protected IHttpClient HttpClient { get; private set; }
		#endregion

		#region [Public methods]
		/// <summary> HTTP post asynchronous. </summary>
		/// <exception cref="AggregateException"> Thrown when an Aggregate error condition occurs. </exception>
		/// <param name="postParameters"> Options for controlling the post. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result that yields a string. </returns>
		/// <seealso cref="M:TSoft.TVServer.HttpClientBase.HttpPostAsync(Dictionary{string,string},CancellationToken)"/>
		public async override Task<string> HttpPostAsync(Dictionary<string, string> postParameters, CancellationToken cancellationToken)
		{
			if (!this.ClientInitialized)
				this.ClientInitialized = true;

			var request = new HttpRequestOptions()
			{
				Url = this.Url,
				RequestContentType = "application/x-www-form-urlencoded",
				LogErrorResponseBody = false,
				LogRequest = false,
				CancellationToken = cancellationToken,
				EnableDefaultUserAgent = true,
				EnableHttpCompression = false,
			};

			if (this.RequiresAuthentication)
			{
				string authInfo = String.Format("{0}:{1}", this.UserName, this.Password);
				authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
				request.RequestHeaders["Authorization"] = "Basic " + authInfo;
			}

			try
			{
				request.SetPostData(postParameters);
				using (var stream = await HttpClient.Post(request).ConfigureAwait(false))
				{
					using (var reader = new StreamReader(stream.Content, UTF8Encoding.UTF8))
					{
						string responseFromServer = reader.ReadToEnd().Trim();
						return responseFromServer;
					}
				}
			}
			catch (AggregateException aggregateException)
			{
				var exception = aggregateException.Flatten().InnerExceptions.OfType<MediaBrowser.Model.Net.HttpException>().FirstOrDefault();

				throw;
			}
			catch (Exception e)
			{
				this._Logger.ErrorException("Http Client error :{0}", e);
				throw;
			}
		}
		#endregion
	}
}
