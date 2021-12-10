using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class IntegrationEvents
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }

        public IntegrationEvents()
        {
            Id= Guid.NewGuid();
            CreationDate = DateTime.Now;
        }

        public IntegrationEvents(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }

    }
}
