using System;
using Akka.Actor;

namespace Hamlet.Core.Actors
{
    public class FunctorActor<T> : ReceiveActor
    {
        private Action<T> _action;

        public FunctorActor(Action<T> action)
        {
            _action = action;

            Receive<T>(action);
        }
    }

    public class FunctorActor<TInput, TOutput> : PubSubActor
    {
        public FunctorActor(Func<TInput, TOutput> converterFunc)
        {
            Receive<TInput>(message =>
            {
                var result = converterFunc(message);
                Publish(result);
            });

            HandleSubscriptionMessagesFor<TOutput>();
        }
    }

    public class FunctorActor : ReceiveActorWithLoggingSupport
    {
        private readonly Action _action;

        public FunctorActor(Action action)
        {
            _action = action;
        }

        protected override void PreStart()
        {
            try
            {
                _action();
            }
            catch (Exception exception)
            {
                LogError($"Exception thrown: {exception}");
                throw;
            }
        }
    }
}