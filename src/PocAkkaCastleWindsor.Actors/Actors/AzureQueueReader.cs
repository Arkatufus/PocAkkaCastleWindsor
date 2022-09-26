using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Microsoft.Extensions.DependencyInjection;
using PocAkkaCastleWindsor.Actors.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAkkaCastleWindsor.Actors.Actors
{
    public class AzureQueueReader : ReceiveActor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public AzureQueueReader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            Context.ActorOf(DependencyResolver.For(Context.System).Props<AzureQueueReader<CompletedOrderMessage, Fetch>>(
                _serviceProvider.GetRequiredService<AzureQueueReader<CompletedOrderMessage, Fetch>>()
            ), "ActorPaths.QueueReaderCompletedOrder.Name");

            //Context.ActorOf(DependencyResolver.For(Context.System).Props<AzureQueueReader<CompletedOrderMessage, Fetch>>(
            //), "ActorPaths.QueueReaderCompletedOrder.Name");
        }

        #region Actor lifecycle methods

        protected override void PreStart()
        {
            _logger.Debug("AzureQueueReader PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("AzureQueueReader PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug("AzureQueueReader PreRestart because {0}", reason);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug("AzureQueueReader PostRestart because {0}", reason);

            base.PostRestart(reason);
        }

        #endregion  
    }

    public class AzureQueueReader<T, TTrigger> : ReceiveActor where T : QueueMessageBase where TTrigger : new()
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly IAzureQueueReaderConfigurator<T> _azureQueueReaderConfigurator;

        public AzureQueueReader(IAzureQueueReaderConfigurator<T> azureQueueReaderConfigurator)
        {
            _azureQueueReaderConfigurator = azureQueueReaderConfigurator;
        }

        #region Actor lifecycle methods

        protected override void PreStart()
        {
            _logger.Debug("AzureQueueReader<T, TTrigger> PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("AzureQueueReader<T, TTrigger> PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug("AzureQueueReader<T, TTrigger> PreRestart because {0}", reason);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug("AzureQueueReader<T, TTrigger> PostRestart because {0}", reason);

            base.PostRestart(reason);
        }

        #endregion
    }

    public class Fetch { }

    public class DefaultAzureQueueReaderConfigurator<T> : IAzureQueueReaderConfigurator<T> where T : QueueMessageBase
    {
        private readonly Action<T, IUntypedActorContext> _actionOnDequeue;

        public DefaultAzureQueueReaderConfigurator(Action<T, IUntypedActorContext> actionOnDequeue)
        {
            _actionOnDequeue = actionOnDequeue;
        }

        public virtual string QueueName => "";

        public virtual int BatchSize => 1;

        public virtual bool WaitForReadyMessage => false;

        public Action<T, IUntypedActorContext> ActionOnDequeue => _actionOnDequeue;
    }

    public class CompletedOrderMessageAzureQueueReaderConfigurator :
        DefaultAzureQueueReaderConfigurator<CompletedOrderMessage>
    {
        public CompletedOrderMessageAzureQueueReaderConfigurator() :
            base(new Action<CompletedOrderMessage, IUntypedActorContext>((m, c) =>
            {

            }))
        { }

        public override int BatchSize
        {
            get
            {
                return 0;
            }
        }

        public override string QueueName
        {
            get 
            {
                return "";
            }
            
        }
    }

}
