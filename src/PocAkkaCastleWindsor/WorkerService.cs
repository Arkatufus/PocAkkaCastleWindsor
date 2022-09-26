using Akka.Actor;
using Akka.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PocAkkaCastleWindsor.Actors.Actors;
using PocAkkaCastleWindsor.Actors.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PocAkkaCastleWindsor
{
    public class WorkerService : IHostedService
    {
        private ActorSystem _actorSystem;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // get castle container
            var container = BootstrapContainer.GetContainer();

            _actorSystem = container.Resolve<ActorSystem>();
            ConfigureTopLevelActor(_actorSystem);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Trace.TraceInformation("PocAkkaCastleWindsor is stopping");

            // Shutdown the Actor System
            await _actorSystem.Terminate();

            // Wait till the system is fully brought down
            _actorSystem.WhenTerminated.Wait();

            Trace.TraceInformation("PocAkkaCastleWindsor has stopped");
        }

        private void ConfigureTopLevelActor(ActorSystem actorSystem)
        {
            actorSystem.ActorOf(DependencyResolver.For(actorSystem).Props<AzureQueueReader>(), "AzureQueueReader");
        }
    }
}
