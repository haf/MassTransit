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

using MassTransit.Util;

namespace MassTransit.Transports
{
    using System;
    using Diagnostics.Introspection;

	/// <summary>
	/// Implementors are responsible for creating endpoints based on passed uris.
	/// </summary>
    public interface IEndpointFactory :
        DiagnosticsSource,
        IDisposable
    {
        /// <summary>
        /// Creates a new endpoint for the specified URI
        /// </summary>
        /// <param name="uri">To uri to create the endpoint for</param>
        /// <returns>The endpoint instance that was created from the uri</returns>
        IEndpoint CreateEndpoint([NotNull] Uri uri);

        /// <summary>
        /// Adds a transport factory to the endpoint factory
        /// </summary>
        /// <param name="factory">Factory to add</param>
        void AddTransportFactory([NotNull] ITransportFactory factory);
    }
}