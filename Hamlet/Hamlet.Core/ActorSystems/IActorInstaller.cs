using Akka.Actor;

namespace Hamlet.Core.ActorSystems
{
    public interface IActorInstaller
    {
        void InstallActorsInto(ActorSystem actorSystem);
    }
}