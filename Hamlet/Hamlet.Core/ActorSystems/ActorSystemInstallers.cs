using System.Collections.Generic;
using LinFu.Loaders;

namespace Hamlet.Core.ActorSystems
{
    public static class ActorSystemInstallers
    {
        public static IActorInstaller[] LoadFrom(string moduleDirectory)
        {
            var installers = new List<IActorInstaller>();
            installers.LoadFrom(moduleDirectory,"*.dll");
            
            return installers.ToArray();
        }
    }
}