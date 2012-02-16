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
		private readonly ILog _self;

		/// <summary>
		/// Create a new NLog logger instance.
		/// </summary>
		/// <param name="logger">Name of type to log as.</param>
		public NLogLog([NotNull] Logger logger)
		{
			if (logger == null) throw new ArgumentNullException("logger");
			_log = logger;
			_self = this;
		}

		#region IsXXX

		bool ILog.IsDebugEnabled
		{
			get { return _log.IsDebugEnabled; }
		}

		bool ILog.IsInfoEnabled
		{
			get { return _log.IsInfoEnabled; }
		}

		bool ILog.IsWarnEnabled
		{
			get { return _log.IsWarnEnabled; }
		}

		bool ILog.IsErrorEnabled
		{
			get { return _log.IsErrorEnabled; }
		}

		bool ILog.IsFatalEnabled
		{
			get { return _log.IsFatalEnabled; }
		}

		#endregion

		#region Logging Methods

		void ILog.Debug(object obj)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Debug, _log.Name, null, "{0}", new[] {obj}));
		}

		void ILog.Debug(LogMessageGenerator messageGenerator)
		{
			if (_self.IsDebugEnabled)
				_self.Debug(messageGenerator());
		}

		void ILog.Debug(object obj, Exception exception)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Debug, _log.Name, null, "{0}", new[] {obj}, exception));
		}

		void ILog.Info(object obj)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Info, _log.Name, null, "{0}", new[] {obj}));
		}

		void ILog.Info(LogMessageGenerator messageGenerator)
		{
			if (_self.IsInfoEnabled)
				_self.Info(messageGenerator());
		}

		void ILog.Info(object obj, Exception exception)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Info, _log.Name, null, "{0}", new[] {obj}, exception));
		}

		void ILog.Warn(object obj)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Warn, _log.Name, null, "{0}", new[] {obj}));
		}

		void ILog.Warn(LogMessageGenerator messageGenerator)
		{
			if (_self.IsWarnEnabled)
				_self.Warn(messageGenerator());
		}

		void ILog.Warn(object obj, Exception exception)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Warn, _log.Name, null, "{0}", new[] {obj}, exception));
		}

		void ILog.Error(object obj)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Error, _log.Name, null, "{0}", new[] {obj}));
		}

		void ILog.Error(LogMessageGenerator messageGenerator)
		{
			if (_self.IsErrorEnabled)
				_self.Warn(messageGenerator());
		}

		void ILog.Error(object obj, Exception exception)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Error, _log.Name, null, "{0}", new[] {obj}, exception));
		}

		void ILog.Fatal(object obj)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Fatal, _log.Name, null, "{0}", new[] {obj}));
		}

		void ILog.Fatal(LogMessageGenerator messageGenerator)
		{
			if (_self.IsFatalEnabled)
				_self.Fatal(messageGenerator());
		}

		void ILog.Fatal(object obj, Exception exception)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Fatal, _log.Name, null, "{0}", new[] {obj}, exception));
		}

		#endregion

		#region Formatting Members

		void ILog.DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Debug, _log.Name, formatProvider, format, args));
		}

		void ILog.DebugFormat(string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Debug, _log.Name, null, format, args));
		}

		void ILog.InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Info, _log.Name, formatProvider, format, args));
		}

		void ILog.InfoFormat(string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Info, _log.Name, null, format, args));
		}

		void ILog.WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Warn, _log.Name, formatProvider, format, args));
		}

		void ILog.WarnFormat(string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Warn, _log.Name, null, format, args));
		}

		void ILog.ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Error, _log.Name, formatProvider, format, args));
		}

		void ILog.ErrorFormat(string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Error, _log.Name, null, format, args));
		}

		void ILog.FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Fatal, _log.Name, formatProvider, format, args));
		}

		void ILog.FatalFormat(string format, params object[] args)
		{
			_log.Log(typeof (NLogLog), new LogEventInfo(LogLevel.Fatal, _log.Name, null, format, args));
		}

		#endregion
	}
}