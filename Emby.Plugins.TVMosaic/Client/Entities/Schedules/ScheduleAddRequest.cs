using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

using System.Xml.Serialization;

namespace TSoft.TVServer.Entities
{
    /// <summary> A schedule add request. </summary>
    /// <seealso cref="T:TSoft.TVServer.Interfaces.IRequest"/>
    [XmlRoot("schedule")]
    public class ScheduleAddRequest : Schedule, Interfaces.IRequest
    {
        #region [Public properties]
        /// <summary> Gets the HTTP command. </summary>
        /// <value> The HTTP command. </value>
        /// <seealso cref="P:TSoft.TVServer.Interfaces.IRequest.HttpCommand"/>
        public string HttpCommand
        {
            get { return "add_schedule"; }
        }
        #endregion
    }
}
