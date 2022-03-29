// <copyright file="PluginLogger.cs" >
// Copyright (c) 2017 Tavares Software Developement. All rights reserved.
// </copyright>
// <author>Tavares André</author>
// <date>01.09.2017</date>
// <summary>Implements the plugin logger class</summary>
using System;
using System.Text;
using TSoft.TVServer.Helpers;
using MediaBrowser.Model.Logging;

namespace Emby.Plugins.DVBLogic.Helpers
{
	/// <summary> A plugin logger. </summary>
	/// <seealso cref="T:TSoft.TVServer.Helpers.AbstractLogger"/>
	public class PluginLogger : AbstractLogger
	{
		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the Emby.Plugins.DVBLink.Helpers.PluginLogger class. </summary>
		/// <param name="logger"> The logger. </param>
		public PluginLogger(ILogger logger, string name)
		{
			this._logger = logger;
			this._name = name;
			this._logMessage = $"[{this._name}]" + " - {0}";
		}
		#endregion

		#region [Fields]

		/// <summary> The logger. </summary>
		private readonly ILogger _logger;

		/// <summary> The name. </summary>
		private readonly string _name;

		private string _logMessage = "";
		#endregion

		#region [Public methods]
		/// <summary> Debugs. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.Debug(string,params object[])"/>
		public override void Debug(string message, params object[] paramList)
		{
			this._logger.Debug(this._logMessage, string.Format(message, paramList));
		}

		/// <summary> Errors. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.Error(string,params object[])"/>
		public override void Error(string message, params object[] paramList)
		{
			this._logger.Error(this._logMessage, string.Format(message, paramList));
		}

		/// <summary> Error exception. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="exception"> The exception. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.ErrorException(string,Exception,params object[])"/>
		public override void ErrorException(string message, Exception exception, params object[] paramList)
		{
			this._logger.FatalException(this._logMessage, exception, string.Format(message, paramList));
		}

		/// <summary> Fatals. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.Fatal(string,params object[])"/>
		public override void Fatal(string message, params object[] paramList)
		{
			this._logger.Fatal(this._logMessage, string.Format(message, paramList));
		}

		/// <summary> Fatal exception. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="exception"> The exception. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.FatalException(string,Exception,params object[])"/>
		public override void FatalException(string message, Exception exception, params object[] paramList)
		{
			this._logger.FatalException(this._logMessage, exception, string.Format(message, paramList));
		}

		/// <summary> Infoes. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.Info(string,params object[])"/>
		public override void Info(string message, params object[] paramList)
		{
			this._logger.Info(this._logMessage, string.Format(message, paramList));
		}

		/// <summary> Logs. </summary>
		/// <param name="severity"> The severity. </param>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.Log(TSoft.TVServer.Constants.LogSeverity,string,params object[])"/>
		public override void Log(TSoft.TVServer.Constants.LogSeverity severity, string message, params object[] paramList)
		{
			this._logger.Log((LogSeverity)severity, this._logMessage, string.Format(message, paramList));
		}

		/// <summary> Logs a multiline. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="severity"> The severity. </param>
		/// <param name="additionalContent"> The additional content. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.LogMultiline(string,TSoft.TVServer.Constants.LogSeverity,StringBuilder)"/>
		public override void LogMultiline(string message, TSoft.TVServer.Constants.LogSeverity severity, StringBuilder additionalContent)
		{
			this._logger.LogMultiline(string.Format(this._logMessage, message), (LogSeverity)severity, additionalContent);
		}

		/// <summary> Warns. </summary>
		/// <param name="message"> The message. </param>
		/// <param name="paramList"> A variable-length parameters list containing parameter list. </param>
		/// <seealso cref="M:TSoft.TVServer.Helpers.AbstractLogger.Warn(string,params object[])"/>
		public override void Warn(string message, params object[] paramList)
		{
			this._logger.Warn(this._logMessage, string.Format(message, paramList));
		}
		#endregion
	}
}
