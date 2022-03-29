using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;
using TSoft.TVServer.Constants;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> A schedule. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
	[XmlRoot("schedule")]
    public class Schedule
    {
        #region [Constructors]
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.Schedule class. </summary>
        /// <param name="by_epg"> The by epg. </param>
        /// <param name="user_param"> The user parameter. </param>
        /// <param name="is_force_add"> true if this TSoft.TVServer.Entities.Schedule is force
        /// add. </param>
        public Schedule(ByEpgSchedule by_epg, string user_param, bool is_force_add)
        {
            ByEpg = by_epg;
            UserParam = user_param;
            ForceAdd = is_force_add;
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.Schedule class. </summary>
        /// <param name="manual"> The manual. </param>
        /// <param name="user_param"> The user parameter. </param>
        /// <param name="is_force_add"> true if this TSoft.TVServer.Entities.Schedule is force
        /// add. </param>
		public Schedule(ManualSchedule manual, string user_param, bool is_force_add)
        {
            Manual = manual;
            UserParam = user_param;
            ForceAdd = is_force_add;
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.Schedule class. </summary>
        /// <param name="manual"> The manual. </param>
		public Schedule(ManualSchedule manual)
            : this(manual, null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.Schedule class. </summary>
        /// <param name="by_epg"> The by epg. </param>
        /// <param name="user_param"> The user parameter. </param>
		public Schedule(ByEpgSchedule by_epg, string user_param)
            : this(by_epg, user_param, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.Schedule class. </summary>
		public Schedule()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.Schedule class. </summary>
        /// <param name="manual"> The manual. </param>
        /// <param name="user_param"> The user parameter. </param>
		public Schedule(ManualSchedule manual, string user_param)
            : this(manual, user_param, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.Schedule class. </summary>
        /// <param name="by_epg"> The by epg. </param>
		public Schedule(ByEpgSchedule by_epg)
            : this(by_epg, null, false)
        {
        }

        #endregion

        #region [Public Properties]

        /// <summary> Gets or sets the identifier of the schedule. </summary>
        /// <value> The identifier of the schedule. </value>
        [XmlElement(ElementName = "schedule_id")]
        public string ScheduleID { get; set; }

        /// <summary> Gets or sets the user parameter. </summary>
        /// <value> The user parameter. </value>
		[XmlElement(ElementName = "user_param"), DefaultValue("")]
        public string UserParam { get; set; }

        /// <summary> Gets or sets a value indicating whether the add should be forced. </summary>
        /// <value> true if force add, false if not. </value>
		[XmlElement(ElementName = "force_add")]
        public bool ForceAdd { get; set; }

        /// <summary> Gets or sets the margine before. </summary>
        /// <value> The margine before. </value>
		[XmlElement(ElementName = "margine_before"), DefaultValue(-1)]
        public int MargineBefore { get; set; }

        /// <summary> Gets or sets the margine after. </summary>
        /// <value> The margine after. </value>
		[XmlElement(ElementName = "margine_after"), DefaultValue(-1)]
        public int MargineAfter { get; set; }

		/// <summary> Gets or sets the priority. </summary>
		/// <value> The priority. </value>
		[XmlElement(ElementName = "priority")]
		public EnumSchedulePriority Priority { get; set; }

		[XmlElement(ElementName = "active")]
		public bool Active { get; set; }

		/// <summary> Gets or sets the by epg. </summary>
		/// <value> The by epg. </value>
		[XmlElement(ElementName = "by_epg")]
        public ByEpgSchedule ByEpg { get; set; }

        /// <summary> Gets or sets the manual. </summary>
        /// <value> The manual. </value>
		[XmlElement(ElementName = "manual")]
        public ManualSchedule Manual { get; set; }

        /// <summary> Gets or sets the by patern. </summary>
        /// <value> The by patern. </value>
        [XmlElement(ElementName = "by_pattern")]
        public PatternSchedule ByPatern { get; set; }

        #endregion

    }
}
