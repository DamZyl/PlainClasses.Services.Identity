using System;
using System.Collections.Generic;
using PlainClasses.Services.Identity.Domain.SharedKernels;

namespace PlainClasses.Services.Identity.Domain.Models
{
    public class Person : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }
        public string PersonalNumber { get; private set; }
        public string MilitaryRankAcr { get; private set; }
        public string Password { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        private ISet<PersonAuth> _personAuths = new HashSet<PersonAuth>();
        public IEnumerable<PersonAuth> PersonAuths => _personAuths;
    }
}