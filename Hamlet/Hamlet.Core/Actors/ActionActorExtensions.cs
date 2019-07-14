using System;
using Akka.Actor;

namespace Hamlet.Core.Actors
{
    public static class ActionActorExtensions
    {
        public static IActorRef RunAsSupervisedActor(this Action action, IActorRefFactory actorRefFactory)
        {
            return RunAsSupervisedActor(action, actorRefFactory, Guid.NewGuid().ToString());
        }

        public static IActorRef RunAsSupervisedActor(this Action action, IActorRefFactory actorRefFactory,
            string childName,
            Func<SupervisorStrategy> getSupervisorStrategy = null)
        {
            var childProps = Props.Create(() => new FunctorActor(action));
            var supervisorProps = Props.Create(() => new SupervisorActor(childName, childProps, getSupervisorStrategy));

            return actorRefFactory.ActorOf(supervisorProps);
        }

        public static IActorRef RunAsSupervisedActor<T>(this Action<T> action, IActorRefFactory actorRefFactory)
        {
            return RunAsSupervisedActor(action, actorRefFactory, Guid.NewGuid().ToString());
        }

        public static IActorRef RunAsSupervisedActor<T>(this Action<T> action, IActorRefFactory actorRefFactory,
            string childName, Func<SupervisorStrategy> getSupervisorStrategy = null)
        {
            var childProps = Props.Create(() => new FunctorActor<T>(action));
            var supervisorProps = Props.Create(() => new SupervisorActor(childName, childProps, getSupervisorStrategy));

            return actorRefFactory.ActorOf(supervisorProps);
        }

        public static IActorRef RunAsUnsupervisedActor<T>(this Action<T> action, IActorRefFactory actorRefFactory)
        {
            return actorRefFactory.ActorOf(Props.Create(() => new FunctorActor<T>(action)));
        }

        public static IActorRef ConvertFuncToActor<TInput, TOutput>(this Func<TInput, TOutput> functor,
            ActorSystem actorSystem)
        {
            return actorSystem.ActorOf(Props.Create(() => new FunctorActor<TInput, TOutput>(functor)));
        }

        public static IActorRef ConvertActionToActor<TMessage>(this Action<TMessage> functor, ActorSystem actorSystem)
        {
            return actorSystem.ActorOf(Props.Create(() => new FunctorActor<TMessage>(functor)));
        }
    }
}