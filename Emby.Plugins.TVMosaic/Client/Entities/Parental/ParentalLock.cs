using System;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> A parental lock. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
	[XmlRoot("parental_lock")]
    public class ParentalLock : IRequest
	{
        #region [Constructors]
        /// <summary> Initializes a new instance of the <see cref="ParentalLock"/> class. </summary>
        /// <param name="clientId"> . </param>
        /// <param name="isEnable"> . </param>
        /// <param name="lockCode"> . </param>
        public ParentalLock(string clientId, bool isEnable, string lockCode)
        {
            ClientId = clientId;
            IsEnable = isEnable;
            LockCode = lockCode;
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ParentalLock class. </summary>
		public ParentalLock()
		{
		}
		#endregion

		#region [Public Properties]
        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "set_parental_lock"; }
        }

        /// <summary> Gets or sets the identifier of the client. </summary>
        /// <value> The identifier of the client. </value>
		[XmlElement(ElementName = "client_id"), DefaultValue("")]
		public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this TSoft.TVServer.Entities.ParentalLock is
        /// enable. </summary>
        /// <value>
        /// true if this TSoft.TVServer.Entities.ParentalLock is enable, false if not. </value>
		[XmlElement(ElementName = "is_enable"), DefaultValue(false)]
		public bool IsEnable { get; set; }

        /// <summary> Gets or sets the lock code. </summary>
        /// <value> The lock code. </value>
		[XmlElement(ElementName = "code"), DefaultValue("")]
		public string LockCode { get; set; }

		#endregion

	}
}
