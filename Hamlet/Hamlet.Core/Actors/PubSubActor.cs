using System;
using System.Collections.Generic;
using Akka.Actor;
using Hamlet.Core.Messages;

namespace Hamlet.Core.Actors
{
    public abstract class PubSubActor : ReceiveActorWithLoggingSupport
    {
        private readonly Dictionary<Type, HashSet<IActorRef>> _typedSubscribers =
            new Dictionary<Type, HashSet<IActorRef>>();

        private readonly HashSet<IActorRef> _untypedSubscribers = new HashSet<IActorRef>();
        
        protected PubSubActor()
        {
            Receive<Subscribe>(message => { AddSubscriber(message.Subscriber); });
        }

        protected void HandleSubscriptionMessagesFor<TMessage>()
        {
            Receive<SubscribeTo<TMessage>>(message =>
            {
                var subscriber = message.Subscriber;

                AddSubscriber(subscriber);
            });

            Receive<UnsubscribeFrom<TMessage>>(message =>
            {
                var subscriber = message.Subscriber;

                RemoveSubscriber<TMessage>(subscriber);
            });
        }

        protected void Publish<TMessage>(TMessage message)
        {
            // Publish the message to the untyped subscribers
            foreach (var subscriber in _untypedSubscribers)
            {
                if (subscriber == null)
                    continue;

                Context.System.Scheduler.ScheduleTellOnce(TimeSpan.Zero, subscriber, message, Self);
            }

            // Publish the message to the typed subscribers
            var messageType = typeof(TMessage);
            if (!_typedSubscribers.ContainsKey(messageType))
                return;

            var subscribers = _typedSubscribers[messageType];
            foreach (var subscriber in subscribers)
            {
                if (subscriber == null)
                    continue;

                Context.System.Scheduler.ScheduleTellOnce(TimeSpan.Zero, subscriber, message, Self);
            }
        }

        private void AddSubscriber(IActorRef subscriber)
        {
            if (_untypedSubscribers.Contains(subscriber))
                return;

            _untypedSubscribers.Add(subscriber);
        }

        private void RemoveSubscriber<TMessage>(IActorRef subscriber)
        {
            var messageType = typeof(TMessage);
            if (!_typedSubscribers.ContainsKey(messageType))
                return;

            var subscribers = _typedSubscribers[messageType];
            if (subscribers.Contains(subscriber))
                subscribers.Remove(subscriber);
        }


        private void ClearAllSubscribers()
        {
            _untypedSubscribers?.Clear();
            _typedSubscribers.Clear();
        }

        protected override void PostStop()
        {
            ClearAllSubscribers();
        }
    }
}