using Akka.Actor;
using Akka.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using PocAkkaCastleWindsor.Actors.Actors;
using PocAkkaCastleWindsor.Actors.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAkkaCastleWindsor.Actors.Installers
{
    public class ActorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Akka config
            container.Register(
                Component.For<Config>()
                    .OnlyNewServices()
                    .UsingFactoryMethod(() => AzureRuntimeBootloader.CreateConfig()));
            // init and register the actor system itself
            container.Register(
                Component.For<ActorSystem>()
                    .UsingFactoryMethod((kernel) =>
                    {
                        var config = kernel.Resolve<Config>(); //get azure config
                        var sys = SystemFactory.Create(kernel.Resolve<IServiceProvider>(), config, container); // create actor system
                        return sys;
                    }));

            // Registration of Generic Actor (Actor constructor being invoked but Prestart method failing)
            container.Register(
                Component.For<IQueue>().ImplementedBy<AzureQueue>(),
                Component.For<AzureQueueReader<CompletedOrderMessage, Fetch>>()
                    .DependsOn(Dependency.OnValue<IAzureQueueReaderConfigurator<CompletedOrderMessage>>(
                            new CompletedOrderMessageAzureQueueReaderConfigurator()),
                        Property.ForKey<TimeSpan>().Eq(TimeSpan.FromSeconds(30)),
                        Property.ForKey<bool>().Eq(true)));
        }

    }
}
