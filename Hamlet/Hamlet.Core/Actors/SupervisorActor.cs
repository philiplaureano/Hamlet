using System;
using Akka.Actor;

namespace Hamlet.Core.Actors
{
    internal class SupervisorActor : ReceiveActor
    {
        private readonly string _childName;
        private readonly Props _childProps;
        private readonly Func<SupervisorStrategy> _getSupervisorStrategy;
        private IActorRef _childActor;

        public SupervisorActor(Props childProps) : this(Guid.NewGuid().ToString(), childProps)
        {
        }

        public SupervisorActor(string childName, Props childProps) : this(childName, childProps, null)
        {
        }

        public SupervisorActor(string childName, Props childProps, Func<SupervisorStrategy> getSupervisorStrategy)
        {
            _childName = childName ?? Guid.NewGuid().ToString();
            _childProps = childProps;
            _getSupervisorStrategy = getSupervisorStrategy;
        }

        protected override void PreStart()
        {
            _childActor = Context.ActorOf(_childProps, _childName);
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return _getSupervisorStrategy == null
                ? base.SupervisorStrategy()
                : _getSupervisorStrategy() ?? base.SupervisorStrategy();
        }
    }
}