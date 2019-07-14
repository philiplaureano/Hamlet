using Akka.Actor;
using Akka.Event;

namespace Hamlet.Core.Actors
{
    public abstract class ReceiveActorWithLoggingSupport : ReceiveActor
    {
        protected void LogInfo(string message)
        {
            Logger?.Log(LogLevel.InfoLevel, message);
        }

        protected void LogWarning(string message)
        {
            Logger?.Log(LogLevel.WarningLevel, message);
        }

        protected void LogDebug(string message)
        {
            Logger?.Log(LogLevel.DebugLevel, message);
        }

        protected void LogError(string message)
        {
            Logger?.Log(LogLevel.ErrorLevel, message);
        }

        private ILoggingAdapter Logger => Context.GetLogger();
    }
}