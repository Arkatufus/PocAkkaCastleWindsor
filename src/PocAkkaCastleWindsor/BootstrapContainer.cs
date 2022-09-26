using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;
using Castle.Windsor;
using Castle.Windsor.Installer;
using PocAkkaCastleWindsor.Actors.Installers;
using PocAkkaCastleWindsor.Infrastructure.Installers;

namespace PocAkkaCastleWindsor
{
    public static class BootstrapContainer
    {
        private static IWindsorContainer container;

        static BootstrapContainer()
        {
            if (container == null)
            {
                container = new WindsorContainer();
            }
        }

        public static IWindsorContainer GetContainer()
        {
            if (container != null)
            {
                return container;
            }

            container = new WindsorContainer();
            return container;
        }

        public static void InitializeContainer()
        {
            container.Kernel.ComponentModelBuilder.AddContributor(new TransientEqualizer());

            container.Install(FromAssembly.This(),
                FromAssembly.Containing<WindsorInstallerFactory>(
                    new WindsorInstallerFactory()),
                FromAssembly.Containing<ActorInstaller>());
        }

        public class TransientEqualizer : IContributeComponentModelConstruction
        {
            public void ProcessModel(IKernel kernel,
                ComponentModel model)
            {
                if (model.LifestyleType != LifestyleType.Undefined
                    && model.LifestyleType != LifestyleType.Singleton
                    && model.LifestyleType != LifestyleType.Bound)
                    model.LifestyleType = LifestyleType.Transient;
            }
        }
    }

}
