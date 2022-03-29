// <copyright file="ILogger.cs" >
// Copyright (c) 2015 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares Andr�</author>
// <date>18.12.2015</date>
// <summary>Declares the ILogger interface</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSoft.TVServer.Constants;

namespace TSoft.TVServer.Interfaces
{
	/// <summary> Interface ILogger. </summary>
	public interface ILogger
	{
		#region [Methods Implementation]

		/// <summary> Debugs the specified message. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> The param list. </param>
		void Debug(string message, params object[] paramList);

		/// <summary> Errors the specified message. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> The param list. </param>
		void Error(string message, params object[] paramList);

		/// <summary> Logs the exception. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="exception"> The exception. </param>
		/// <param name="paramList"> The param list. </param>
		void ErrorException(string message, Exception exception, params object[] paramList);

		/// <summary> Fatals the specified message. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> The param list. </param>
		void Fatal(string message, params object[] paramList);

		/// <summary> Fatals the exception. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="exception"> The exception. </param>
		/// <param name="paramList"> The param list. </param>
		void FatalException(string message, Exception exception, params object[] paramList);

		/// <summary> Infoes the specified message. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> The param list. </param>
		void Info(string message, params object[] paramList);

		/// <summary> Logs the specified severity. </summary>
		/// <param name="severity"> The severity. </param>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> The parameter list. </param>
		void Log(LogSeverity severity, string message, params object[] paramList);

		/// <summary> Logs the multiline. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="severity"> The severity. </param>
		/// <param name="additionalContent"> Content of the additional. </param>
		void LogMultiline(string message, LogSeverity severity, StringBuilder additionalContent);

		/// <summary> Warns the specified message. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> The param list. </param>
		void Warn(string message, params object[] paramList);
		#endregion
	}
}
