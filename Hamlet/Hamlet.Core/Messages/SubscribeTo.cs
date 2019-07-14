using System;
using Akka.Actor;

namespace Hamlet.Core.Messages
{
    public class SubscribeTo<TMessage>
    {
        public SubscribeTo(IActorRef subscriber)
        {
            Subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
        }

        public IActorRef Subscriber { get; }
    }
}