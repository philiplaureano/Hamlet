using Akka.Actor;
using Akka.Event;

namespace Hamlet.Core.ActorSystems
{
    public class EventStreamForwarder<TForwardedMessage> : ReceiveActor
    {
        private EventStream _eventStream;

        public EventStreamForwarder(EventStream eventStream)
        {
            _eventStream = eventStream;
            Receive<TForwardedMessage>(msg =>
            {
                // Forward the message to the event bus
                _eventStream?.Publish(msg);
            });
        }
    }
}