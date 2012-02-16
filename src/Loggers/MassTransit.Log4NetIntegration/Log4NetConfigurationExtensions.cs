﻿// Copyright 2007-2012 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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

namespace MassTransit.Log4NetIntegration
{
	using BusConfigurators;
	using Logging;
	using MassTransit.Logging;
	using Util;

	/// <summary>
	/// 	Extensions for configuring MassTransit for log4net
	/// </summary>
	public static class Log4NetConfigurationExtensions
	{
		/// <summary>
		/// 	Specify that you want to use the Log4net logging engine for logging with MassTransit.
		/// </summary>
		/// <param name="configurator"> </param>
		public static void UseLog4Net([CanBeNull] this ServiceBusConfigurator configurator)
		{
			Logger.UseLogger(new Log4NetLogger());
		}
	}
}