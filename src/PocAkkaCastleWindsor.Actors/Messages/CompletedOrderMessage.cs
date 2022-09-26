using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PocAkkaCastleWindsor.Actors.Messages
{
    [DataContract]
    public class CompletedOrderMessage : QueueMessageBase
    {
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public Guid StopId { get; set; }
    }
}
