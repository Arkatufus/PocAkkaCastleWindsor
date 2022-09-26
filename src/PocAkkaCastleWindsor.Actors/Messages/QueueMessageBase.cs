using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PocAkkaCastleWindsor.Actors.Messages
{
    [DataContract]
    public abstract class QueueMessageBase : ICloneable
    {
        [DataMember]
        public Guid UniqueId { get; set; }

        private object _context;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "We would never want this to be serialized")]
        public object GetContext()
        {
            return this._context;
        }

        public void SetContext(object value)
        {
            this._context = value;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
