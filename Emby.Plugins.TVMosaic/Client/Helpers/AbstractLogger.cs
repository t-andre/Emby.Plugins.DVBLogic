using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSoft.TVServer.Constants;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Helpers
{
	/// <summary> An abstract logger. </summary>
	public abstract class AbstractLogger
	{
		#region [Public Methods]

		/// <summary> Debugs. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		public abstract void Debug(string message, params object[] paramList);

		/// <summary> Errors. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		public abstract void Error(string message, params object[] paramList);

		/// <summary> Error exception. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="exception"> The exception. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		public abstract void ErrorException(string message, Exception exception, params object[] paramList);

		/// <summary> Fatals. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		public abstract void Fatal(string message, params object[] paramList);

		/// <summary> Fatal exception. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="exception"> The exception. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		public abstract void FatalException(string message, Exception exception, params object[] paramList);

		/// <summary> Infoes. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		public abstract void Info(string message, params object[] paramList);

		/// <summary> Logs. </summary>
		/// <param name="severity"> The severity. </param>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		public abstract void Log(LogSeverity severity, string message, params object[] paramList);

		/// <summary> Logs a multiline. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="severity"> The severity. </param>
		/// <param name="additionalContent"> The additional content. </param>
		public abstract void LogMultiline(string message, LogSeverity severity, StringBuilder additionalContent);

		/// <summary> Warns. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		public abstract void Warn(string message, params object[] paramList);

		#endregion

	}
}
