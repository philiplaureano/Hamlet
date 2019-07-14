using Akka.Actor;

namespace Hamlet.Core.Messages
{
    public class UnsubscribeFrom<TMessage>
    {
        public UnsubscribeFrom(IActorRef subscriber)
        {
            Subscriber = subscriber;
        }

        public IActorRef Subscriber { get; }
    }
}