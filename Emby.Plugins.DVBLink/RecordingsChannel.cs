// <copyright file="RecordingsChannel.cs" >
// Copyright (c) 2018 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares</author>
// <date>23.08.2018</date>
// <summary>Implements the recordings channel class</summary>
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.MediaInfo;
using MediaBrowser.Controller.LiveTv;
using Emby.Plugins.DVBLogic;

namespace Emby.Plugins.DVBLink
{
	/// <summary> The recordings channel. </summary>
	/// <seealso cref="T:Emby.Plugins.DVBLink.RecordingsChannelBase"/>
	public class RecordingsChannel : RecordingsChannelBase
	{
		#region [Constructors]
		/// <summary> Initializes a new instance of the Emby.Plugins.DVBLink.RecordingsChannel class. </summary>
		/// <param name="liveTvManager"> Manager for live TV. </param>
		public RecordingsChannel(ILiveTvManager liveTvManager) : base(liveTvManager)
		{
		}
		#endregion

		#region [Public properties]
		/// <summary> Gets the data version. </summary>
		/// <value> The data version. </value>
		public override string DataVersion { get { return "1"; } }

		/// <summary> Gets the description. </summary>
		/// <value> The description. </value>
		public override string Description { get { return "DVBLink Recordings"; } }

		/// <summary> Gets URL of the home page. </summary>
		/// <value> The home page URL. </value>
		public override string HomePageUrl { get { return "http://dvblogic.com/"; } }

		/// <summary> Gets the name. </summary>
		/// <value> The name. </value>
		public override string Name { get { return "DVBLink Recordings"; } }
		#endregion

		#region [Public methods]
		/// <summary> Gets channel image. </summary>
		/// <param name="type">				 The type. </param>
		/// <param name="cancellationToken"> A token that allows processing to be cancelled. </param>
		/// <returns> An asynchronous result that yields the channel image. </returns>
		public override Task<DynamicImageResponse> GetChannelImage(ImageType type, CancellationToken cancellationToken)
		{
			if (type == ImageType.Primary)
			{
				return Task.FromResult(new DynamicImageResponse
				{
					Path = "http://dvblogic.com/imgs/files/b_dvblink_logo.jpg",
					Protocol = MediaProtocol.Http,
					HasImage = true
				});
			}

			return Task.FromResult(new DynamicImageResponse
			{
				HasImage = false
			});
		}
		#endregion
	}
}
