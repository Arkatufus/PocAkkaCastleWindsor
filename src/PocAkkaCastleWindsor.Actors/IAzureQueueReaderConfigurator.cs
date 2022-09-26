using Akka.Actor;
using PocAkkaCastleWindsor.Actors.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAkkaCastleWindsor.Actors
{
    public interface IAzureQueueReaderConfigurator<T> where T : QueueMessageBase
    {
        string QueueName { get; }
        int BatchSize { get; }
        bool WaitForReadyMessage { get; }
        Action<T, IUntypedActorContext> ActionOnDequeue { get; }
    }
}
