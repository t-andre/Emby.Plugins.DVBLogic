using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;

namespace TSoft.TVServer.Entities
{
    /// <summary> The schedules request. </summary>
    [XmlRoot("schedules")]
    public class SchedulesRequest : Interfaces.IRequest
    {
        #region [Public properties]
        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "get_schedules"; }
        }
        #endregion
    }
}
