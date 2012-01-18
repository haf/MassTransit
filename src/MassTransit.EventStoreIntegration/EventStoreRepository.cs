﻿// Copyright 2007-2011 Henrik Feldt
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
namespace MassTransit.EventStoreIntegration
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using EventStore;
	using EventStore.Persistence;
	using Exceptions;
	using Magnum;
	using Pipeline;
	using Saga;
	using Util;
	using log4net;

	/// <summary>
	/// This instance has the moving state:
	/// 
	/// <list type="bullet">
	///		<value>Uncommitted events</value>
	///		<value><see cref="ClearUncommittedEvents"/> clears the instance list of uncommitted events.</value>
	/// </list>
	/// </summary>
	public interface IEventSourcedSaga : ISaga
	{
		/// <summary>
		/// Gets the saga version.
		/// </summary>
		ulong Version { get; }

		/// <summary>
		/// Apply an event that causes the saga to transition.
		/// </summary>
		/// <param name="message"></param>
		void Transition(object message);

		/// <summary>
		/// Gets the collection of events that haven't yet been committed.
		/// </summary>
		/// <returns>A collection of events</returns>
		IEnumerable<object> GetUncommittedEvents();

		/// <summary>
		/// Clears the collection of uncommitted events from this saga instance.
		/// </summary>
		void ClearUncommittedEvents();
	}

	/// <summary>
	/// 	joliver's Event Store backing of sagas!
	/// </summary>
	/// <typeparam name="TSaga">The type of saga.</typeparam>
	public class EventStoreRepository<TSaga> :
		ISagaRepository<TSaga>
		where TSaga : class, IEventSourcedSaga, new()
	{
		static readonly ILog _log = LogManager.GetLogger(typeof (InMemorySagaRepository<TSaga>));

		readonly ISagaRepository<TSaga> _self;
		readonly IStoreEvents _eventStore;

		public EventStoreRepository([NotNull] IStoreEvents eventStore)
		{
			if (eventStore == null) throw new ArgumentNullException("eventStore");
			_eventStore = eventStore;
			_self = this;
		}

		IEnumerable<Action<IConsumeContext<TMessage>>> ISagaRepository<TSaga>.GetSaga<TMessage>(
			IConsumeContext<TMessage> context, Guid sagaId,
			InstanceHandlerSelector<TSaga, TMessage> selector,
			ISagaPolicy<TSaga, TMessage> policy)
		{
			TSaga instance;
			IEventStream sagaStream = TryGetSaga(sagaId, out instance);

			if (instance == null)
			{
				if (policy.CanCreateInstance(context))
				{
					yield return x =>
						{
							if (_log.IsDebugEnabled)
								_log.DebugFormat("SAGA: {0} Creating New {1} for {2}", typeof (TSaga).ToFriendlyName(), sagaId,
									typeof (TMessage).ToFriendlyName());

							try
							{
								instance = policy.CreateInstance(x, sagaId);

								// iterate over the saga handlers
								foreach (var callback in selector(instance, x))
									callback(x);

								if (!policy.CanRemoveInstance(instance))
									Save<TMessage>(instance, sagaStream, new Guid(x.RequestId));
							}
							catch (Exception ex)
							{
								var sex = new SagaException("Create Saga Instance Exception", typeof (TSaga), typeof (TMessage), sagaId, ex);
								
								if (_log.IsErrorEnabled)
									_log.Error(sex);

								throw sex;
							}
						};
				}
				else
				{
					if (_log.IsDebugEnabled)
						_log.DebugFormat("SAGA: {0} Ignoring Missing {1} for {2}", typeof(TSaga).ToFriendlyName(), sagaId,
							typeof(TMessage).ToFriendlyName());
				}
			}
			else
			{
				if (policy.CanUseExistingInstance(context))
				{
					yield return x =>
					{
						if (_log.IsDebugEnabled)
							_log.DebugFormat("SAGA: {0} Using Existing {1} for {2}", typeof(TSaga).ToFriendlyName(), sagaId,
								typeof(TMessage).ToFriendlyName());

						try
						{
							foreach (var callback in selector(instance, x))
							{
								callback(x);
							}

							Save<TMessage>(instance, sagaStream, GetCommitId(x));
							
							if (policy.CanRemoveInstance(instance)) ;
								// no need to do work right now...
						}
						catch (Exception ex)
						{
							var sex = new SagaException("Existing Saga Instance Exception", typeof(TSaga), typeof(TMessage), sagaId, ex);
							if (_log.IsErrorEnabled)
								_log.Error(sex);

							throw sex;
						}
					};
				}
				else
				{
					if (_log.IsDebugEnabled)
						_log.DebugFormat("SAGA: {0} Ignoring Existing {1} for {2}", typeof(TSaga).ToFriendlyName(), sagaId,
							typeof(TMessage).ToFriendlyName());
				}
			}
		}

		static Guid GetCommitId<TMessage>(IConsumeContext<TMessage> cc)
			where TMessage : class
		{
			var messageId = cc.MessageId ?? cc.RequestId ?? cc.CorrelationId ?? cc.ConversationId;
			Guid commitId;
			if (!Guid.TryParse(messageId, out commitId)) commitId = CombGuid.Generate();
			return commitId;
		}

		/// <returns>Always an event stream. Maybe a saga instance.</returns>
		IEventStream TryGetSaga(Guid sagaId, out TSaga instance)
		{
			IEventStream eventStream;
			try
			{
				eventStream = _eventStore.OpenStream(sagaId, 0, int.MaxValue);
				instance = new TSaga();

				foreach (var @event in eventStream.CommittedEvents.Select(x => x.Body))
					instance.Transition(@event);
			}
			catch (StreamNotFoundException ex)
			{
				_log.Info(string.Format("could not find saga, creating new event stream for saga #{0}", sagaId), ex);
				eventStream = _eventStore.CreateStream(sagaId);
				instance = null;
			}
			return eventStream;
		}

		void Save<TMessage>(TSaga instance, IEventStream sagaStream, Guid commitId)
		{
			var stream = PrepareStream(instance, new Dictionary<string, object>(), sagaStream);

			Persist<TMessage>(stream, commitId);

			instance.ClearUncommittedEvents();
		}

		private IEventStream PrepareStream(TSaga saga, Dictionary<string, object> headers, IEventStream stream)
		{
			foreach (var item in headers)
				stream.UncommittedHeaders[item.Key] = item.Value;

			saga.GetUncommittedEvents()
				.Cast<object>()
				.Select(x => new EventMessage { Body = x })
				.ToList()
				.ForEach(stream.Add);

			return stream;
		}
		private static void Persist<TMessage>(IEventStream stream, Guid commitId)
		{
			try
			{
				stream.CommitChanges(commitId);
			}
			// also: ConcurrencyException
			catch (DuplicateCommitException)
			{
				stream.ClearChanges();
			}
			catch (StorageException e)
			{
				throw new SagaException(e.Message, typeof(TSaga), typeof(TMessage), commitId, e);
			}
		}

		IEnumerable<Guid> ISagaRepository<TSaga>.Find(ISagaFilter<TSaga> filter)
		{
			return _self.Where(filter, x => x.CorrelationId);
		}

		IEnumerable<TSaga> ISagaRepository<TSaga>.Where(ISagaFilter<TSaga> filter)
		{
			throw new NotSupportedException("the event store doesn't support enumerating sagas");
		}

		IEnumerable<TResult> ISagaRepository<TSaga>.Where<TResult>(ISagaFilter<TSaga> filter,
			Func<TSaga, TResult> transformer)
		{
			return _self.Where(filter).Select(transformer);
		}

		IEnumerable<TResult> ISagaRepository<TSaga>.Select<TResult>(Func<TSaga, TResult> transformer)
		{
			throw new NotSupportedException("the event store doesn't support enumerating sagas");
		}
	}
}