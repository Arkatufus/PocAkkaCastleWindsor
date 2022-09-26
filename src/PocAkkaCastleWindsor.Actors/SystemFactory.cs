using Akka.Actor;
using Akka.Configuration;
using Akka.DependencyInjection;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAkkaCastleWindsor.Actors
{
    public static class SystemFactory
    {
        private static IWindsorContainer _container;

        public static IWindsorContainer Container { get { return _container; } }

        public static ActorSystem Create(IServiceProvider serviceProvider, Config config, IWindsorContainer container)
        {
            _container = container;

            var bootstrap = BootstrapSetup.Create().WithConfig(config);

            // Enable DI support inside this ActorSystem, if needed
            var di = DependencyResolverSetup.Create(serviceProvider);

            // Merge this setup (and any others) together into ActorSystemSetup
            var actorSystemSetup = bootstrap.And(di);

            //var fuseSystem = ActorSystem.Create("fuse", phobosSetup);
            var fuseSystem = ActorSystem.Create("fuse", actorSystemSetup);

            return fuseSystem;
        }
    }
}
