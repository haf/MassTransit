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

namespace MassTransit.Logging
{
    using System;
    using Util;

	/// <summary>
	/// A delegate that only is called if the given level is being logged with. This allows
	/// the programmer to avoid having expensive debugging string format expressions
	/// evaluate.
	/// </summary>
	/// <returns>The string to log.</returns>
	public delegate string LogMessageGenerator();

	/// <summary>
	/// Implementors handle logging and filtering based on logging levels.
	/// </summary>
    public interface ILog
    {
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }

        void Debug(object obj);
		void Debug([NotNull] LogMessageGenerator messageGenerator);
        void Debug([NotNull] object obj, Exception exception);
        void Info(object obj);
		void Info([NotNull] LogMessageGenerator messageGenerator);
		void Info([NotNull] object obj, Exception exception);
        void Warn(object obj);
		void Warn(LogMessageGenerator messageGenerator);
		void Warn([NotNull] object obj, Exception exception);
        void Error(object obj);
		void Error([NotNull] LogMessageGenerator messageGenerator);
		void Error([NotNull] object obj, Exception exception);
        void Fatal(object obj);
		void Fatal([NotNull] LogMessageGenerator messageGenerator);
		void Fatal([NotNull] object obj, Exception exception);
        void DebugFormat(IFormatProvider formatProvider, string format, params object[] args);
        void DebugFormat(string format, params object[] args);

        void InfoFormat(IFormatProvider formatProvider, string format, params object[] args);
        void InfoFormat(string format, params object[] args);

        void WarnFormat(IFormatProvider formatProvider, string format, params object[] args);
        void WarnFormat(string format, params object[] args);

        void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args);
        void ErrorFormat(string format, params object[] args);

        void FatalFormat(IFormatProvider formatProvider, string format, params object[] args);
        void FatalFormat(string format, params object[] args);
    }
}