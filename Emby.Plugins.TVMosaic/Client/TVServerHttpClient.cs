// <copyright file="TVServerHttpClient.cs" >
// Copyright (c) 2018 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares</author>
// <date>27.08.2018</date>
// <summary>Implements the TV server HTTP client class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TSoft.TVServer.Helpers;

namespace TSoft.TVServer
{
	/// <summary> A dvb link HTTP client. </summary>
	/// <seealso cref="T:TSoft.TVServer.HttpClientBase"/>
	public class TVServerHttpClient : HttpClientBase
	{
		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the TSoft.TVServer.DVBLinkHttpClient class. </summary>
		/// <param name="logger"> The logger. </param>
		public TVServerHttpClient(AbstractLogger logger) : base(logger)
		{
		}

		#endregion

		#region [Fields]

		/// <summary> The handler. </summary>
		private HttpClientHandler _Handler;
		/// <summary> The HTTP client. </summary>
		private HttpClient _httpClient;

		#endregion

		#region [Public methods]
		/// <summary> HTTP post asynchronous. </summary>
		/// <param name="postParameters"> Options for controlling the post. </param>
		/// <param name="cancellationToken"> The cancellation token. </param>
		/// <returns> The asynchronous result that yields a string. </returns>
		/// <seealso cref="M:TSoft.TVServer.HttpClientBase.HttpPostAsync(Dictionary{string,string},CancellationToken)"/>
		public async override Task<string> HttpPostAsync(Dictionary<string, string> postParameters, CancellationToken cancellationToken)
		{
			string responseString = string.Empty;

			try
			{
				if (!this.ClientInitialized)
				{
					this._Handler = new HttpClientHandler { Credentials = new NetworkCredential(this.UserName, this.Password) };
					this._httpClient = new HttpClient(this._Handler);
					this.ClientInitialized = true;
				}

				//HttpEncoder.Current = HttpEncoder.Default;
				string postData = string.Empty;
				var strings = postParameters.Keys.Select(key => string.Format("{0}={1}", key, postParameters[key]));
				postData = string.Join("&", strings.ToArray());

				StringContent content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
				using (var response = await this._httpClient.PostAsync(this.Url, content, cancellationToken).ConfigureAwait(false))
				{
					responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				}
			}
			catch (Exception e)
			{
				this._httpClient = null;
				this._Logger.ErrorException("Http Client error :{0}", e);
			}

			return responseString;
		}
		#endregion
	}
}
