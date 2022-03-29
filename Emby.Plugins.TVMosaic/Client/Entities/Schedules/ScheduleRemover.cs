using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Entities
{
    /// <summary> A schedule remover. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
	[XmlRoot("remove_schedule")]
    public class ScheduleRemover : IRequest
	{
		#region [Constructors]
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ScheduleRemover class. </summary>
		public ScheduleRemover()
		{
		}

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.ScheduleRemover class. </summary>
        /// <param name="schedule_id"> Identifier for the schedule. </param>
		public ScheduleRemover(string schedule_id)
		{
			ScheduleID = schedule_id;
		}

		#endregion

		#region [Public Properties]
        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "remove_schedule"; }
        }

        /// <summary> Gets or sets the identifier of the schedule. </summary>
        /// <value> The identifier of the schedule. </value>
		[XmlElement(ElementName = "schedule_id")]
		public string ScheduleID { get; set; }

		#endregion

	}
}
