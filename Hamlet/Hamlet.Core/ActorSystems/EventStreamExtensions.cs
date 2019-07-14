using System;
using Akka.Actor;
using Hamlet.Core.Actors;
using Hamlet.Core.Messages;

namespace Hamlet.Core.ActorSystems
{
    public static class EventStreamExtensions
    {
        public static void AddEventStreamMessageHandler<TMessage>(this ActorSystem actorSystem, Action<TMessage> handler)
        {
            var unsupervisedActor = handler.RunAsUnsupervisedActor(actorSystem);
            actorSystem.ForwardEventsTo<TMessage>(unsupervisedActor);
        }

        public static void ForwardEventsTo<TMessage>(this ActorSystem actorSystem,
            IActorRef subscriber)
        {
            actorSystem.EventStream.Subscribe(subscriber, typeof(TMessage));
        }

        public static void ForwardEventsTo<TMessage>(this IActorRef sourceActor,
            ActorSystem actorSystem)
        {
            if (sourceActor == null) 
                throw new ArgumentNullException(nameof(sourceActor));
            
            if (actorSystem == null) 
                throw new ArgumentNullException(nameof(actorSystem));

            Action<TMessage> messageThunk = msg =>
            {
                actorSystem.EventStream.Publish(msg);
            };

            var adapterActor = messageThunk.ConvertActionToActor(actorSystem);
            sourceActor.Tell(new SubscribeTo<TMessage>(adapterActor));
        }

        public static void ScheduleEventStreamMessage<TMessage>(this ActorSystem actorSystem, TimeSpan delay,
            TMessage message)
        {
            var forwarder = actorSystem.ActorOf(Props.Create(() =>
                new EventStreamForwarder<TMessage>(actorSystem.EventStream)));

            actorSystem.Scheduler.ScheduleTellOnce(delay, forwarder, message, null);
        }
    }
}