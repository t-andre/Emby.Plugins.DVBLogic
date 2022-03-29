using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSoft.TVServer.Constants;

namespace TSoft.TVServer.Entities
{
    /// <summary> A session. </summary>
    public class Session
    {
        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.Session class. </summary>
        public Session()
        {
            StreamStartTime = DateTime.MinValue;
            StreamEndTime = DateTime.MinValue;
            CreatedOn = DateTime.Now;
            UpdatedOn = DateTime.MinValue;
            StreamSource = EnumStreamSourceType.NONE;
            StreamHandle = -1;
        }
        #endregion

        #region [Public properties]
        /// <summary> Gets or sets the identifier of the session. </summary>
        /// <value> The identifier of the session. </value>
        public string SessionId { get; set; }

        /// <summary> Gets or sets the name of the session. </summary>
        /// <value> The name of the session. </value>
        public string SessionName { get; set; }

        /// <summary> Gets or sets the session notes. </summary>
        /// <value> The session notes. </value>
        public string SessionNotes { get; set; }

        /// <summary> Gets or sets the identifier of the device. </summary>
        /// <value> The identifier of the device. </value>
        public string DeviceId { get; set; }

        /// <summary> Gets or sets the name of the device. </summary>
        /// <value> The name of the device. </value>
        public string DeviceName { get; set; }

        /// <summary> Gets or sets the identifier of the client. </summary>
        /// <value> The identifier of the client. </value>
        public string ClientId { get; set; }

        /// <summary> Gets or sets the name of the client. </summary>
        /// <value> The name of the client. </value>
        public string ClientName { get; set; }

        /// <summary> Gets or sets the client IP. </summary>
        /// <value> The client IP. </value>
        public string ClientIP { get; set; }

        /// <summary> Gets or sets the client MAC. </summary>
        /// <value> The client MAC. </value>
        public string ClientMac { get; set; }

        /// <summary> Gets or sets the stream source. </summary>
        /// <value> The stream source. </value>
        public EnumStreamSourceType StreamSource { get; set; }

        /// <summary> Gets or sets the identifier of the user. </summary>
        /// <value> The identifier of the user. </value>
        public string UserId { get; set; }

        /// <summary> Gets or sets the name of the user. </summary>
        /// <value> The name of the user. </value>
        public string UserName { get; set; }

        /// <summary> Gets or sets the identifier of the stream. </summary>
        /// <value> The identifier of the stream. </value>
        public string StreamId { get; set; }

        /// <summary> Gets or sets the name of the stream. </summary>
        /// <value> The name of the stream. </value>
        public string StreamName { get; set; }

        /// <summary> Gets or sets a value indicating whether the stream open. </summary>
        /// <value> true if stream open, false if not. </value>
        public bool StreamOpen { get; set; }

        /// <summary> Gets or sets the handle of the stream. </summary>
        /// <value> The stream handle. </value>
        public long StreamHandle { get; set; }

        /// <summary> Gets or sets URL of the stream. </summary>
        /// <value> The stream URL. </value>
        public string StreamUrl { get; set; }

        /// <summary> Gets or sets the stream start time. </summary>
        /// <value> The stream start time. </value>
        public DateTime StreamStartTime { get; set; }

        /// <summary> Gets or sets the stream end time. </summary>
        /// <value> The stream end time. </value>
        public DateTime StreamEndTime { get; set; }

        /// <summary> Gets the Date/Time of the created on. </summary>
        /// <value> The created on. </value>
        public DateTime CreatedOn { get; private set; }

        /// <summary> Gets or sets the Date/Time of the updated on. </summary>
        /// <value> The updated on. </value>
        public DateTime UpdatedOn { get; set; }
        #endregion
    }
}
