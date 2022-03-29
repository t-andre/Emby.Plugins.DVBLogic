
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSoft.TVServer.Constants;
using TSoft.TVServer.Helpers;

namespace TSoft.TVServer.Entities
{
	/// <summary> Information about the stream source. </summary>
	public class StreamSourceInfo
	{
		#region [Public Properties]

		/// <summary> Gets or sets the identifier. </summary>
		/// <value> The identifier. </value>
		public string Id { get; set; }

		/// <summary> Gets or sets the name. </summary>
		/// <value> The name. </value>
		public string Name { get; set; }

		/// <summary> Gets or sets the transcoder. </summary>
		/// <value> The transcoder. </value>
		public Transcoder Transcoder { get; set; }

		/// <summary> Gets or sets the type of the stream. </summary>
		/// <value> The type of the stream. </value>
		public EnumStreamType StreamType { get; set; }

		/// <summary> Gets or sets a value indicating whether the direct stream request. </summary>
		/// <value> true if direct stream request, false if not. </value>
		public bool DirectStreamRequest { get; set; }

		/// <summary> Gets or sets the profile. </summary>
		/// <value> The profile. </value>
		public EnumProfiles Profile { get; set; }

		/// <summary> Gets or sets a value indicating whether the timeshift. </summary>
		/// <value> true if timeshift, false if not. </value>
		public bool Timeshift { get; set; }

		/// <summary> Gets a value indicating whether the transcode. </summary>
		/// <value> true if transcode, false if not. </value>
		public bool Transcode { get; set; }

		/// <summary>
		/// Gets a value indicating whether this MediaBrowser.Plugins.DVBLink.Models.StreamSourceInfo is
		/// default. </summary>
		/// <value>
		/// true if this MediaBrowser.Plugins.DVBLink.Models.StreamSourceInfo is default, false if not. </value>
		public bool IsDefault { get; set; }

        /// <summary> The state. </summary>
        public EnumState state { get; set; }

        /// <summary>
        /// The start date of the recording, in UTC.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// The end date of the recording, in UTC.
        /// </summary>
        public DateTimeOffset EndDate { get; set; }
        #endregion
    }
}
