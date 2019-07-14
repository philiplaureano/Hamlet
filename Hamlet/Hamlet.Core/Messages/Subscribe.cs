using System;
using Akka.Actor;

namespace Hamlet.Core.Messages
{
    public class Subscribe
    {
        public Subscribe(IActorRef subscriber)
        {
            Subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
        }

        public IActorRef Subscriber { get; }
    }
}