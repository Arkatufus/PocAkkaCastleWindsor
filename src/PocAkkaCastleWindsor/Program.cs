using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PocAkkaCastleWindsor
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // Methods below have to be invoked in sequence. After
            // the upgrade to Akka.DependencyInjection and in order
            // to use the same IWindsorInstallers (instead of rewriting
            // the composition root from scratch) we added
            // Castle.Windsor.Extensions.DependencyInjection to
            // the project and merged it with the Microsoft DI container

            // WindsorContainer Singleton Instantiated
            var container = BootstrapContainer.GetContainer();

            // WindsorContainer merged with Microsoft DI
            var host = Host.CreateDefaultBuilder(args)
                .UseWindsorContainerServiceProvider(container)
                .ConfigureServices(ConfigureServices);

            // WindsorInstallers will be invoked.
            BootstrapContainer.InitializeContainer();

            return host;
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<WorkerService>();
        }
    }
}
