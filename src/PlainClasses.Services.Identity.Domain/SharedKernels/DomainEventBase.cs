using System;

namespace PlainClasses.Services.Identity.Domain.SharedKernels
{
    public class DomainEventBase : IDomainEvent
    {
        public DateTime OccurredOn { get; }
        
        public DomainEventBase()
        {
            OccurredOn = DateTime.Now;
        }
    }
}