using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSoft.TVServer.Constants;
using TSoft.TVServer.Interfaces;

namespace TSoft.TVServer.Helpers
{
	/// <summary> A proxie logger. </summary>
	/// <seealso cref="T:TSoft.TVServer.Helpers.AbstractLogger"/>
	public class ProxieLogger : AbstractLogger
	{
		#region [Constructors]

		/// <summary>
		/// Initializes a new instance of the TSoft.TVServer.Helpers.ProxieLogger class. </summary>
		/// <param name="logger"> The logger. </param>
		public ProxieLogger(AbstractLogger logger)
		{
			_logger = logger;
		}
		#endregion

		#region [Fields]

		/// <summary> The logger. </summary>
		readonly AbstractLogger _logger;
		#endregion

		#region [Public methods]

		/// <summary> Debugs. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.Debug(string,params object[])"/>
		public override void Debug(string message, params object[] paramList)
		{
			_logger.Debug("[DVBLink] - {0}", string.Format(message, paramList));
		}

		/// <summary> Errors. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.Error(string,params object[])"/>
		public override void Error(string message, params object[] paramList)
		{
			_logger.Error("[DVBLink] - {0}", string.Format(message, paramList));
		}

		/// <summary> Error exception. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="exception"> The exception. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.ErrorException(string,Exception,params object[])"/>
		public override void ErrorException(string message, Exception exception, params object[] paramList)
		{
			_logger.FatalException("[DVBLink] - {0}", exception, string.Format(message, paramList));
		}

		/// <summary> Fatals. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.Fatal(string,params object[])"/>
		public override void Fatal(string message, params object[] paramList)
		{
			_logger.Fatal("[DVBLink] - {0}", string.Format(message, paramList));
		}

		/// <summary> Fatal exception. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="exception"> The exception. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.FatalException(string,Exception,params object[])"/>
		public override void FatalException(string message, Exception exception, params object[] paramList)
		{
			_logger.FatalException("[DVBLink] - {0}", exception, string.Format(message, paramList));
		}

		/// <summary> Infos. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.Info(string,params object[])"/>
		public override void Info(string message, params object[] paramList)
		{
			_logger.Info("[DVBLink] - {0}", string.Format(message, paramList));
		}

		/// <summary> Logs. </summary>
		/// <param name="severity"> The severity. </param>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.Log(LogSeverity,string,params object[])"/>
		public override void Log(LogSeverity severity, string message, params object[] paramList)
		{
			_logger.Log(severity, "[DVBLink] - {0}", string.Format(message, paramList));
		}

		/// <summary> Logs a multiline. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="severity"> The severity. </param>
		/// <param name="additionalContent"> The additional content. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.LogMultiline(string,LogSeverity,StringBuilder)"/>
		public override void LogMultiline(string message, LogSeverity severity, StringBuilder additionalContent)
		{
			_logger.LogMultiline(string.Format("[DVBLink] - {0}", message), severity, additionalContent);
		}

		/// <summary> Warns. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.Warn(string,params object[])"/>
		public override void Warn(string message, params object[] paramList)
		{
			_logger.Warn("[DVBLink] - {0}", string.Format(message, paramList));
		}
		#endregion

	}
}
