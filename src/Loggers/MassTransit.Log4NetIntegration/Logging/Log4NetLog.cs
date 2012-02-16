// Copyright 2007-2011 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Log4NetIntegration.Logging
{
    using System;
    using MassTransit.Logging;

    public class Log4NetLog :
        ILog
    {
		private readonly ILog _self;
        readonly log4net.ILog _log;

        public Log4NetLog(log4net.ILog log)
        {
            _log = log;
        	_self = this;
        }

    	void ILog.Debug(object message)
        {
            _log.Debug(message);
        }

    	void ILog.Debug(LogMessageGenerator messageGenerator)
    	{
    		if (_self.IsDebugEnabled)
				_self.Debug(messageGenerator());
    	}

    	void ILog.Debug(object message, Exception exception)
        {
            _log.Debug(message, exception);
        }

    	void ILog.DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }

    	void ILog.DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.DebugFormat(provider, format, args);
        }

    	void ILog.Info(object message)
        {
            _log.Info(message);
        }

    	void ILog.Info(LogMessageGenerator messageGenerator)
    	{
			if (_self.IsInfoEnabled)
				_self.Info(messageGenerator());
    	}

    	void ILog.Info(object message, Exception exception)
        {
            _log.Info(message, exception);
        }

    	void ILog.InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }

    	void ILog.InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.InfoFormat(provider, format, args);
        }

    	void ILog.Warn(object message)
        {
            _log.Warn(message);
        }

    	void ILog.Warn(LogMessageGenerator messageGenerator)
    	{
			if (_self.IsWarnEnabled)
				_self.Warn(messageGenerator());
    	}

    	void ILog.Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }

    	void ILog.WarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }

    	void ILog.WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.WarnFormat(provider, format, args);
        }

    	void ILog.Error(object message)
        {
            _log.Error(message);
        }

    	public void Error(LogMessageGenerator messageGenerator)
    	{
			if (_self.IsErrorEnabled)
				_self.Error(messageGenerator());
    	}

    	void ILog.Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }

    	void ILog.ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }

    	void ILog.ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.ErrorFormat(provider, format, args);
        }

    	void ILog.Fatal(object message)
        {
            _log.Fatal(message);
        }

    	void ILog.Fatal(LogMessageGenerator messageGenerator)
    	{
			if (_self.IsFatalEnabled)
				_self.Fatal(messageGenerator());
    	}

    	void ILog.Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

    	void ILog.FatalFormat(string format, params object[] args)
        {
            _log.FatalFormat(format, args);
        }

    	void ILog.FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.FatalFormat(provider, format, args);
        }

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
    }
}