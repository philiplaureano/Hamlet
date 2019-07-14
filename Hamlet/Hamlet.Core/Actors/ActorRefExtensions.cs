using Akka.Actor;
using Akka.Routing;
using Hamlet.Core.Messages;

namespace Hamlet.Core.Actors
{
    public static class ActorRefExtensions
    {
        public static SubscribeTo<TMessage> WillSubscribeTo<TMessage>(this IActorRef actorRef)
        {
            return new SubscribeTo<TMessage>(actorRef);
        }

        public static UnsubscribeFrom<TMessage> WillUnsubscribeFrom<TMessage>(this IActorRef actorRef)
        {
            return new UnsubscribeFrom<TMessage>(actorRef);
        }
        
        public static void BroadcastMessage(this IActorRef actorRef, object message)
        {
            actorRef?.Tell(new Broadcast(message));
        }
    }
}