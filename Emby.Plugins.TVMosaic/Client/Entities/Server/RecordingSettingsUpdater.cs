using System;
using System.Net;

using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
	[XmlRoot("set_recording_settings")]
    public class RecordingSettingsUpdater : IRequest
    {
		#region [Constructors]

		public RecordingSettingsUpdater()
		{
		}

		#endregion

		#region [Public Properties]

        public string HttpCommand
        {
            get { return "set_recording_settings"; }
        }

        /// <summary> Gets or sets the before margin. </summary>
        /// <value> The before margin. </value>
        [XmlElement(ElementName = "before_margin"), DefaultValue(0)]
        public int BeforeMargin { get; set; }

        /// <summary> Gets or sets the after margin. </summary>
        /// <value> The after margin. </value>
		[XmlElement(ElementName = "after_margin"), DefaultValue(0)]
        public int AfterMargin { get; set; }

        /// <summary> Gets or sets the full pathname of the recording file. </summary>
        /// <value> The full pathname of the recording file. </value>
		[XmlElement(ElementName = "recording_path"), DefaultValue("")]
        public string RecordingPath { get; set; }

        /// <summary> Gets or sets a value indicating whether the ds automatic mode. </summary>
        /// <value> true if ds automatic mode, false if not. </value>
        [XmlElement(ElementName = "ds_auto_mode"), DefaultValue(false)]
        public bool DsAutoMode { get; set; }

        /// <summary> Gets or sets a value indicating whether the ds manager value. </summary>
        /// <value> true if ds manager value, false if not. </value>
        [XmlElement(ElementName = "ds_man_value"), DefaultValue(0)]
        public long DsManValue { get; set; }

        /// <summary> Gets or sets a value indicating whether the automatic delete. </summary>
        /// <value> true if automatic delete, false if not. </value>
        [XmlElement(ElementName = "auto_delete"), DefaultValue(false)]
        public bool AutoDelete { get; set; }

        /// <summary> Gets or sets a value indicating whether the new only algo type. </summary>
        /// <value> true if new only algo type, false if not. </value>
        [XmlElement(ElementName = "new_only_algo_type"), DefaultValue(0)]
        public int NewOnlyAlgoType { get; set; }

        #endregion

    }
}
