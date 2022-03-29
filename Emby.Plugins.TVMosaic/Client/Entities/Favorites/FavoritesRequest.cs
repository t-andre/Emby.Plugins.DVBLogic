using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
	/// <summary> A server information request. </summary>
	/// <seealso cref="T:MediaBrowser.Plugins.DVBLink.Models.IRequest"/>
	[XmlRoot("favorites")]
	public class FavoritesRequest : IRequest
	{
		#region [Public properties]
		/// <summary> Gets the HTTP command. </summary>
		/// <value> The HTTP command. </value>
		/// <seealso cref="P:MediaBrowser.Plugins.DVBLink.Models.IRequest.HttpCommand"/>
		public string HttpCommand
		{
			get { return "get_favorites"; }
		}
		#endregion
	}
}
