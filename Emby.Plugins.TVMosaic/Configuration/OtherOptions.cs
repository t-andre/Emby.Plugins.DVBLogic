// <copyright file="OtherOptions.cs" >
// Copyright (c) 2017 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>01.09.2017</date>
// <summary>Implements the other options class</summary>
using System;
using System.Linq;

namespace Emby.Plugins.DVBLogic.Configuration
{
	/// <summary> An other options. </summary>
	public class OtherOptions
	{
		#region [Public properties]

		/// <summary> Gets or sets a value indicating whether the interlaced video should be forced. </summary>
		/// <value> True if force interlaced video, false if not. </value>
		public Boolean ForceInterlacedVideo { get; set; }

		/// <summary> Gets or sets a value indicating whether the probe live stream. </summary>
		/// <value> True if probe live stream, false if not. </value>
		public Boolean ProbeLiveStream { get; set; }

		/// <summary> Gets or sets a value indicating whether the probe recording stream. </summary>
		/// <value> True if probe recording stream, false if not. </value>
		public Boolean ProbeRecordingStream { get; set; }

		/// <summary> Gets or sets the favourite 1. </summary>
		/// <value> The favourite 1. </value>
		public string Favourite1 { get; set; } = "-1";

		/// <summary> Gets or sets the favourite 2. </summary>
		/// <value> The favourite 2. </value>
		public string Favourite2 { get; set; } = "-1";

		/// <summary> Gets or sets the favourite 3. </summary>
		/// <value> The favourite 3. </value>
		public string Favourite3 { get; set; } = "-1";

		/// <summary> Gets or sets the favourite 4. </summary>
		/// <value> The favourite 4. </value>
		public string Favourite4 { get; set; } = "-1";

		/// <summary> Gets or sets the favourite 5. </summary>
		/// <value> The favourite 5. </value>
		public string Favourite5 { get; set; } = "-1";
		#endregion
	}
}
