using System;
using MassTransit.Logging;
using MassTransit.Util;
using NLog;
using LogMessageGenerator = MassTransit.Logging.LogMessageGenerator;

namespace MassTransit.NLogIntegration.Logging
{
	using Logger = NLog.Logger;

	/// <summary>
	/// A logger that wraps to NLog. See http://stackoverflow.com/questions/7412156/how-to-retain-callsite-information-when-wrapping-nlog
	/// </summary>
	public class NLogLog : ILog
	{
		private readonly Logger _log;

		/// <summary>
		/// Create a new NLog logger instance.
		/// </summary>
		/// <param name="logger">Name of type to log as.</param>
		public NLogLog([NotNull] Logger logger)
		{
			if (logger == null) throw new ArgumentNullException("logger");
			_log = logger;
		}

		#region IsXXX

		public bool IsDebugEnabled
		{
			get { return _log.IsDebugEnabled; }
		}

		public bool IsInfoEnabled
		{
			get { return _log.IsInfoEnabled; }
		}

		public bool IsWarnEnabled
		{
			get { return _log.IsWarnEnabled; }
		}

		public bool IsErrorEnabled
		{
			get { return _log.IsErrorEnabled; }
		}

		public bool IsFatalEnabled
		{
			get { return _log.IsFatalEnabled; }
		}

		#endregion

		#region Logging Methods

		public void Debug(object obj)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Debug, _log.Name, null, "{0}", new[] {obj}));
		}

		public void Debug(LogMessageGenerator messageGenerator)
		{
			if (IsDebugEnabled)
				Debug(messageGenerator());
		}

		public void Debug(object obj, Exception exception)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Debug, _log.Name, null, "{0}", new[] {obj}, exception));
		}

		public void Info(object obj)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Info, _log.Name, null, "{0}", new[] {obj}));
		}

		public void Info(LogMessageGenerator messageGenerator)
		{
			if (IsInfoEnabled)
				Info(messageGenerator());
		}

		public void Info(object obj, Exception exception)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Info, _log.Name, null, "{0}", new[] {obj}, exception));
		}

		public void Warn(object obj)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Warn, _log.Name, null, "{0}", new[] {obj}));
		}

		public void Warn(LogMessageGenerator messageGenerator)
		{
			if (IsWarnEnabled)
				Warn(messageGenerator());
		}

		public void Warn(object obj, Exception exception)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Warn, _log.Name, null, "{0}", new[] {obj}, exception));
		}

		public void Error(object obj)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Error, _log.Name, null, "{0}", new[] {obj}));
		}

		public void Error(LogMessageGenerator messageGenerator)
		{
			if (IsErrorEnabled)
				Warn(messageGenerator());
		}

		public void Error(object obj, Exception exception)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Error, _log.Name, null, "{0}", new[] {obj}, exception));
		}

		public void Fatal(object obj)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Fatal, _log.Name, null, "{0}", new[] {obj}));
		}

		public void Fatal(LogMessageGenerator messageGenerator)
		{
			if (IsFatalEnabled)
				Fatal(messageGenerator());
		}

		public void Fatal(object obj, Exception exception)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Fatal, _log.Name, null, "{0}", new[] {obj}, exception));
		}

		#endregion

		#region Formatting Members

		public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Debug, _log.Name, formatProvider, format, args));
		}

		public void DebugFormat(string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Debug, _log.Name, null, format, args));
		}

		public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Info, _log.Name, formatProvider, format, args));
		}

		public void InfoFormat(string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Info, _log.Name, null, format, args));
		}

		public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Warn, _log.Name, formatProvider, format, args));
		}

		public void WarnFormat(string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Warn, _log.Name, null, format, args));
		}

		public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Error, _log.Name, formatProvider, format, args));
		}

		public void ErrorFormat(string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Error, _log.Name, null, format, args));
		}

		public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Fatal, _log.Name, formatProvider, format, args));
		}

		public void FatalFormat(string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Fatal, _log.Name, null, format, args));
		}

		#endregion
	}
}