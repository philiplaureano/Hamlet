using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akka.Actor;
using Akka.Configuration;
using Akka.Event;

namespace Hamlet.Core.ActorSystems
{
    public static class ActorSystemHost
    {
        public static ActorSystem CreateActorSystem(LogLevel logLevel = LogLevel.DebugLevel,
            int dispatcherThroughPut = 25)
        {
            var entries = new Dictionary<string, string>();

            void SetKey(string key, object value)
            {
                entries[key] = value?.ToString();
            }

            var level = logLevel.ToString().ToUpperInvariant().Replace("LEVEL", string.Empty);
            SetKey("akka.stdout-loglevel", level);
            SetKey("akka.loglevel", level);

            if (logLevel == LogLevel.DebugLevel)
            {
                SetKey("akka.log-config-on-start", "on");
                SetKey("akka.actor.debug.receive", "on");
                SetKey("akka.actor.debug.autoreceive", "on");
                SetKey("akka.actor.debug.lifecycle", "on");
                SetKey("akka.actor.debug.event-stream", "on");
                SetKey("akka.actor.debug.unhandled", "on");
            }

            SetKey("akka.actor.default-dispatcher.throughput", dispatcherThroughPut);

            var keyVauePairs = new Queue<KeyValuePair<string, string>>(entries.ToArray());
            var hoconBuilder = new StringBuilder();
            while (keyVauePairs.Count > 0)
            {
                var currentPair = keyVauePairs.Dequeue();
                hoconBuilder.AppendLine($"{currentPair.Key} = {currentPair.Value}");
            }

            var hocon = hoconBuilder.ToString();
            var finalConfig = ConfigurationFactory.ParseString(hocon);

            var actorSystem = ActorSystem.Create($"ActorSystem-{Guid.NewGuid().ToString()}", finalConfig);
            return actorSystem;
        }

        public static ActorSystem RunActorSystem(LogLevel logLevel = LogLevel.InfoLevel,
            params IActorInstaller[] installers)
        {
            var actions =
                installers.Select(installer => (Action<ActorSystem>) installer.InstallActorsInto).ToArray();

            return RunActorSystem(logLevel, actions);
        }
        
        public static ActorSystem RunActorSystem(LogLevel logLevel = LogLevel.InfoLevel,
            params Action<ActorSystem>[] installers)
        {
            var actorSystem = CreateActorSystem(logLevel);
            foreach (var installer in installers)
            {
                installer?.Invoke(actorSystem);
            }

            return actorSystem;
        }

        public static ActorSystem RunActorSystemFrom(string moduleDirectory, LogLevel logLevel = LogLevel.InfoLevel)
        {
            var installers = ActorSystemInstallers.LoadFrom(moduleDirectory);
            return RunActorSystem(logLevel, installers);
        }
    }
}