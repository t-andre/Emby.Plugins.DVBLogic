// <copyright file="LogSeverity.cs" >
// Copyright (c) 2015 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>18.12.2015</date>
// <summary>Implements the log severity class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSoft.TVServer.Constants
{
    /// <summary> Enum LogSeverity. </summary>
    public enum LogSeverity
    {
        /// <summary>
        /// The info
        /// </summary>
        Info,

        /// <summary>
        /// The debug
        /// </summary>
        Debug,

        /// <summary>
        /// The warn
        /// </summary>
        Warn,

        /// <summary>
        /// The error
        /// </summary>
        Error,

        /// <summary>
        /// The fatal
        /// </summary>
        Fatal
    }
}
