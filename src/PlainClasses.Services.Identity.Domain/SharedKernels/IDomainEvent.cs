using System;

namespace PlainClasses.Services.Identity.Domain.SharedKernels
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}