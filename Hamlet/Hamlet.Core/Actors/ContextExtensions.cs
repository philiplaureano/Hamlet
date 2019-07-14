using System;
using Akka.Actor;

namespace Hamlet.Core.Actors
{
    public static class ContextExtensions
    {
        public static void ScheduleTellRepeatedly(this IUntypedActorContext context, TimeSpan initialDelay,
            TimeSpan interval, IActorRef receiver, object message, IActorRef sender)
        {
            context?.System?.Scheduler?.ScheduleTellRepeatedly(initialDelay, interval, 
                receiver, message, sender);
        }
        public static void ScheduleTellOnce(this IUntypedActorContext context, TimeSpan initialDelay,
            TimeSpan interval, IActorRef receiver, object message, IActorRef sender)
        {
            context?.System?.Scheduler?.ScheduleTellOnce(initialDelay, 
                receiver, message, sender);
        }
    }
}