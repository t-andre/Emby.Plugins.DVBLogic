using System;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> A parental status request. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
	[XmlRoot("parental_lock")]
    public class ParentalStatusRequest : IRequest
	{
		#region [Constructors]
		public ParentalStatusRequest(string client_id)
		{
            ClientId = client_id;
		}

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ParentalStatusRequest
        /// class. </summary>
		public ParentalStatusRequest()
		{
		}
        #endregion

        #region [Public Properties]
        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "get_parental_status"; }
        }

        /// <summary> Gets or sets the identifier of the client. </summary>
        /// <value> The identifier of the client. </value>
		[XmlElement(ElementName = "client_id"), DefaultValue("")]
		public string ClientId { get; set; }

		#endregion

	}
}
